﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{

    /// <summary>
    /// Serializer Class.
    /// </summary>
    public class PhpSerializer
    {
        //types:
        // N = null
        // s = string
        // i = int
        // d = double
        // a = array (hashtable)

        private Dictionary<Hashtable, bool> seenHashtables; //for serialize (to infinte prevent loops)
        private Dictionary<ArrayList, bool> seenArrayLists; //for serialize (to infinte prevent loops) lol

        private int pos; //for unserialize

        public bool XMLSafe = true; //This member tells the serializer wether or not to strip carriage returns from strings when serializing and adding them back in when deserializing
                                    //http://www.w3.org/TR/REC-xml/#sec-line-ends

        public Encoding StringEncoding = new System.Text.UTF8Encoding();

        private System.Globalization.NumberFormatInfo nfi;

        public PhpSerializer()
        {
            nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberGroupSeparator = "";
            nfi.NumberDecimalSeparator = ".";
        }

        /// <summary>
        /// Serialize the given object using PHP serialize algorithm
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize(object obj)
        {
            seenArrayLists = new Dictionary<ArrayList, bool>();
            seenHashtables = new Dictionary<Hashtable, bool>();

            return serialize(obj, new StringBuilder()).ToString();
        }

        private StringBuilder serialize(object obj, StringBuilder sb)
        {
            if (obj == null)
            {
                return sb.Append("N;");
            }
            else if (obj is string)
            {
                string str = (string)obj;
                if (XMLSafe)
                {
                    str = str.Replace("\r\n", "\n");
                    str = str.Replace("\r", "\n");//replace \r not followed by \n with a single \n  Should we do this?
                }
                return sb.Append("s:" + StringEncoding.GetByteCount(str) + ":\"" + str + "\";");
            }
            else if (obj is bool)
            {
                return sb.Append("b:" + (((bool)obj) ? "1" : "0") + ";");
            }
            else if (obj is int)
            {
                int i = (int)obj;
                return sb.Append("i:" + i.ToString(nfi) + ";");
            }
            else if (obj is double)
            {
                double d = (double)obj;

                return sb.Append("d:" + d.ToString(nfi) + ";");
            }
            else if (obj is ArrayList)
            {
                if (seenArrayLists.ContainsKey((ArrayList)obj))
                    return sb.Append("N;");//cycle detected
                else
                    seenArrayLists.Add((ArrayList)obj, true);

                ArrayList a = (ArrayList)obj;
                sb.Append("a:" + a.Count + ":{");
                for (int i = 0; i < a.Count; i++)
                {
                    serialize(i, sb);
                    serialize(a[i], sb);
                }
                sb.Append("}");
                return sb;
            }
            else if (obj is Hashtable)
            {
                if (seenHashtables.ContainsKey((Hashtable)obj))
                    return sb.Append("N;");//cycle detected
                else
                    seenHashtables.Add((Hashtable)obj, true);

                Hashtable a = (Hashtable)obj;
                sb.Append("a:" + a.Count + ":{");
                foreach (DictionaryEntry entry in a)
                {
                    serialize(entry.Key, sb);
                    serialize(entry.Value, sb);
                }
                sb.Append("}");
                return sb;
            }
            else
            {
                return sb;
            }
        }//Serialize(object obj)

        public Hashtable Deserialize(string str)
        {
            pos = 0;
            return (Hashtable)deserialize(str);
        }//Deserialize(string str)

        private object deserialize(string str)
        {
            if (str == null || str.Length <= pos)
                return new Object();

            int start, end, length;
            string stLen;
            switch (str[pos])
            {
                case 'N':
                    pos += 2;
                    return null;
                case 'b':
                    char chBool;
                    chBool = str[pos + 2];
                    pos += 4;
                    return chBool == '1';
                case 'i':
                    string stInt;
                    start = str.IndexOf(":", pos) + 1;
                    end = str.IndexOf(";", start);
                    stInt = str.Substring(start, end - start);
                    pos += 3 + stInt.Length;
                    return Int32.Parse(stInt, nfi);
                case 'd':
                    string stDouble;
                    start = str.IndexOf(":", pos) + 1;
                    end = str.IndexOf(";", start);
                    stDouble = str.Substring(start, end - start);
                    pos += 3 + stDouble.Length;
                    return Double.Parse(stDouble, nfi);
                case 's':
                    start = str.IndexOf(":", pos) + 1;
                    end = str.IndexOf(":", start);
                    stLen = str.Substring(start, end - start);
                    int bytelen = Int32.Parse(stLen);
                    length = bytelen;
                    //This is the byte length, not the character length - so we migth  
                    //need to shorten it before usage. This also implies bounds checking
                    if ((end + 2 + length) >= str.Length) length = str.Length - 2 - end;
                    string stRet = str.Substring(end + 2, length);
                    while (StringEncoding.GetByteCount(stRet) > bytelen)
                    {
                        length--;
                        stRet = str.Substring(end + 2, length);
                    }
                    pos += 6 + stLen.Length + length;
                    if (XMLSafe)
                    {
                        stRet = stRet.Replace("\n", "\r\n");
                    }
                    return stRet;
                case 'a':
                    //if keys are ints 0 through N, returns an ArrayList, else returns Hashtable
                    start = str.IndexOf(":", pos) + 1;
                    end = str.IndexOf(":", start);
                    stLen = str.Substring(start, end - start);
                    length = Int32.Parse(stLen);
                    Hashtable htRet = new Hashtable(length);
                    ArrayList alRet = new ArrayList(length);
                    pos += 4 + stLen.Length; //a:Len:{
                    for (int i = 0; i < length; i++)
                    {
                        //read key
                        object key = deserialize(str);
                        //read value
                        object val = deserialize(str);

                        if (alRet != null)
                        {
                            if (key is int && (int)key == alRet.Count)
                                alRet.Add(val);
                            else
                                alRet = null;
                        }
                        htRet[key] = val;
                    }
                    pos++; //skip the }
                    if (pos < str.Length && str[pos] == ';')//skipping our old extra array semi-colon bug (er... php's weirdness)
                        pos++;
                    if (alRet != null)
                        return alRet;
                    else
                        return htRet;
                default:
                    return "";
            }//switch
        }//unserialzie(object)	
    }//class Serializer


    public class PhpDataObject
    {
        protected Hashtable Data = new Hashtable();

        public Hashtable UserData
        {
            get
            {
                if (Data["userData"] == null)
                {
                    Data["userData"] = new Hashtable();
                }
                return (Hashtable)Data["userData"];
            }
            set
            {
                Data["userData"] = value;
            }
        }

        public Hashtable LocalData
        {
            get
            {
                if (Data["localData"] == null)
                {
                    Data["localData"] = new Hashtable();
                }
                return (Hashtable)Data["localData"];
            }
            set
            {
                Data["localData"] = value;
            }
        }

        public string CreatorID
        {
            get
            {
                return (string)Data["creatorId"];
            }
            set
            {
                Data["creatorId"] = value;
            }
        }

        public string NewCatalogID
        {
            get
            {
                return (string)Data["newCatalogId"];
            }
            set
            {
                Data["newCatalogId"] = value;
            }
        }

        public string Comments
        {
            get
            {
                return (string)Data["comments"];
            }
            set
            {
                Data["comments"] = value;
            }
        }

        public string CatalogID
        {
            get
            {
                return(string)UserData["catalogId"];       
            }
            set
            {
                UserData["catalogId"] = value;
            }
        }

        public string uPhaseID
        {
            get
            {
                return (string)UserData["phaseId"];
            }
            set
            {
                UserData["phaseId"] = value;
            }
        }

        public string uSupplierKpsID
        {
            get
            {
                return (string)UserData["supplier_kpsid"];
            }
            set
            {
                UserData["supplier_kpsid"] = value;
            }
        }

        public string uSecretariatKpsID
        {
            get
            {
                return (string)UserData["secretariat_kpsid"];
            }
            set
            {
                UserData["secretariat_kpsid"] = value;
            }
        }

        public string uBookKpsID
        {
            get
            {
                return (string)UserData["book_kpsid"];
            }
            set
            {
                UserData["book_kpsid"] = value;
            }
        }

        public string uBooksCount
        {
            get
            {
                return (string)UserData["booksCount"];
            }
            set
            {
                UserData["booksCount"] = value;
            }
        }

        public string DiscountID
        {
            get
            {
                return (string)LocalData["dicount_id"];
            }
            set
            {
                LocalData["discount_id"] = value;
            }
        }

        public string BookId
        {
            get
            {
                return(string)LocalData["book_id"];
            }
            set
            {
                LocalData["book_id"] = value;
            }
        }

        public string Price
        {
            get
            {
                return (string)LocalData["price"];
            }
            set
            {
                LocalData["price"] = value;
            }
        }

        public string DepartmentID
        {
            get
            {
                return (string)LocalData["department_id"];
            }
            set
            {
                LocalData["department_id"] = value;
            }
        }

        public string BookCount
        {
            get
            {
                return (string)LocalData["book_count"];
            }
            set
            {
                LocalData["book_count"] = value;
            }
        }

        public string SupplierID
        {
            get
            {
                return (string)LocalData["supplier_id"];
            }
            set
            {
                LocalData["supplier_id"] = value;
            }
        }


        public string BookPriceID
        {
            get
            {
                return (string)LocalData["bookprice_id"];
            }
            set
            {
                LocalData["bookprice_id"] = value;
            }
        }

        public string PhaseID
        {
            get
            {
                return(string)LocalData["phase_id"];
            }
            set
            {
                LocalData["phase_id"] = value;
            }
        }

        public string Percentage
        {
            get
            {
                return (string)LocalData["percentage"];
            }
            set
            {
                LocalData["percentage"] = value;
            }
        }

        public decimal CatalogPrice
        {
            get
            {
                return (decimal)LocalData["catalogPrice"];
            }
            set
            {
                LocalData["catalogPrice"] = value;
            }
        }

        public string OldBookCount
        {
            get
            {
                return (string)Data["oldBookCount"];
            }
            set
            {
                Data["oldBookCount"] = value;
            }
        }

        public string UserBookCount
        {
            get
            {
                return (string)Data["userBookCount"];
            }
            set
            {
                Data["userBookCount"] = value;
            }
        }

        public int? BookWasPriced
        {
            get
            {
                return (int?)Data["bookWasPriced"];
            }
            set
            {
                Data["bookWasPriced"] = value;
            }
        }

        public decimal? OldCatalogPrice
        {
            get
            {
                return (decimal)Data["oldCatalogPrice"];
            }
            set
            {
                Data["oldCatalogPrice"] = value;
            }
        }

        public decimal? NewCatalogPrice
        {
            get
            {
                return (decimal?)Data["newCatalogPrice"];
            }
            set
            {
                Data["newCatalogPrice"] = value;
            }
        }

        public Hashtable GetData()
        {
            return Data;
        }

    }
}
