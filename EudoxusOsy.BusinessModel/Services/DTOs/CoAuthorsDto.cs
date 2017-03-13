using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EudoxusOsy.BusinessModel
{
    [DataContract]
    public class CoAuthorsDto
    {
        [DataMember(Name = "kpsBookId")]
        public long BookKpsID { get; set; }

        [DataMember(Name = "timestamp")]
        public long TimeStamp { get; set; }

        [DataMember(Name = "coauthors")]
        public List<CoAuthorDTO> CoAuthors { get; set; }
    }

    [DataContract]
    public class CoAuthorDTO
    {
        [DataMember(Name = "coauthorId")]
        public int CoAuthorID { get; set; }

        [DataMember(Name = "percentage")]
        public int Percentage { get; set; }
    }

}
