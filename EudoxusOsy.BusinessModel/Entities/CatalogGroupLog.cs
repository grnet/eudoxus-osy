using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class CatalogGroupLog
    {
        public enCatalogGroupState NewState
        {
            get { return (enCatalogGroupState)NewStateInt; }
            set
            {
                if (NewStateInt != (int)value)
                    NewStateInt = (int)value;
            }
        }

        public enCatalogGroupState OldState
        {
            get { return (enCatalogGroupState)OldStateInt; }
            set
            {
                if (OldStateInt != (int)value)

                    OldStateInt = (int)value;
            }
        }

        public CatalogGroupChangeValues GetOldValues()
        {
            return new Serializer<CatalogGroupChangeValues>().Deserialize(OldValuesXml);
        }

        public void SetOldValues(CatalogGroupChangeValues values)
        {
            OldValuesXml = new Serializer<CatalogGroupChangeValues>().Serialize(values);
        }

        public CatalogGroupChangeValues GetNewValues()
        {
            return new Serializer<CatalogGroupChangeValues>().Deserialize(NewValuesXml);
        }

        public void SetNewValues(CatalogGroupChangeValues values)
        {
            NewValuesXml = new Serializer<CatalogGroupChangeValues>().Serialize(values);
        }
    }
}
