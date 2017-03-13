using System.Xml.Serialization;

namespace EudoxusOsy.Services.Models
{
    [XmlRoot("Book")]
    public class KpsBookDto
    {
        [XmlAttribute]
        public int idKPS { get; set; }
        [XmlAttribute]
        public int bibNumber { get; set; }
        public string bsaStatus { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string description { get; set; }
        public string authors { get; set; }
        public string isbn { get; set; }
        public VolumeDto volume { get; set; }
        public string previousVolume { get; set; }
        public string nextVolume { get; set; }
        public int publicationNumber { get; set; }
        public int publicationYear { get; set; }
        public int previousEdition { get; set; }
        public int nextEdition { get; set; }
        public string editorialHouse { get; set; }
        public int supplierCode { get; set; }
        public string publisher { get; set; }
        public string dimensions { get; set; }
        public int pages { get; set; }
        public string pathToCover { get; set; }
        public string pathToBackcover { get; set; }
        public string pathToChapter { get; set; }
        public string pathToTOC { get; set; }
        public string linkToEH { get; set; }
        public string binding { get; set; }
        public string kindbook { get; set; }
    }
}
