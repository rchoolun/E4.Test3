using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebApplication1.Models
{
    [XmlRoot(ElementName = "Students")]
    public class StudentCollection
    {
        [XmlElement(ElementName = "Student")]
        public List<Student> Students { get; set; }
    }

    public class Student
    {
        [XmlAttribute("Id")]
        public Guid Id { get; set; }

        [XmlAttribute("Name")]
        [Required]
        public string Name { get; set; }

        [XmlAttribute("Surname")]
        [Required]
        public string Surname { get; set; }

        [XmlAttribute("Phone")]
        [Required]
        public string Phone { get; set; }
    }
}