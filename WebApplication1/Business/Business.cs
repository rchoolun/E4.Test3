using System;
using System.Configuration;
using System.IO;
using System.Xml;
using WebApplication1.Models;

namespace WebApplication1.Business
{
    public class Business
    {
        XmlDocument data;

        public Business()
        {
            data = new XmlDocument();
            data.Load(ConfigurationManager.AppSettings["Datasource"]);
        }

        /// <summary>
        /// Gets all users from XML and converts to a list of users
        /// </summary>
        /// <returns></returns>
        public StudentCollection GetUsers()
        {
            try
            {
                var userList = DeserializeToObject<StudentCollection>(ConfigurationManager.AppSettings["Datasource"]);
                return userList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // <summary>
        /// Gets all users from XML and converts to a list of users
        /// </summary>
        /// <returns></returns>
        public Student GetUserById(Guid id)
        {
            try
            {
                var userList = DeserializeToObject<StudentCollection>(ConfigurationManager.AppSettings["Datasource"]);
                return userList.Students.Find(x => x.Id == id);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Created a new user in the XML
        /// </summary>
        /// <param name="newStudent"></param>
        /// <returns></returns>
        public Status AddStudent(Student newStudent)
        {
            try
            {
                XmlNode root1 = data.DocumentElement;

                XmlElement XEle = data.CreateElement("Student");
                XEle.SetAttribute("Id", newStudent.Id.ToString());
                XEle.SetAttribute("Name", newStudent.Name);
                XEle.SetAttribute("Surname", newStudent.Surname);
                XEle.SetAttribute("Phone", newStudent.Phone);
                data.DocumentElement.AppendChild(XEle.Clone());
                data.Save(ConfigurationManager.AppSettings["Datasource"]);

                return new Status { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new Status { IsSuccess = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Update existing user in XML
        /// </summary>
        /// <param name="studentToUpdate"></param>
        /// <returns></returns>
        public Status UpdateStudent(Student studentToUpdate)
        {
            try
            {
                XmlNode nodeToUpdate = data.SelectSingleNode("/Students/Student[@Id='" + studentToUpdate.Id + "']");

                if (nodeToUpdate != null)
                {
                    nodeToUpdate.Attributes["Name"].Value = studentToUpdate.Name;
                    nodeToUpdate.Attributes["Surname"].Value = studentToUpdate.Surname;
                    nodeToUpdate.Attributes["Phone"].Value = studentToUpdate.Phone;
                }
                data.Save(ConfigurationManager.AppSettings["Datasource"]);

                return new Status { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new Status { IsSuccess = false, Message = ex.Message };
            }

        }

        /// <summary>
        /// Deletes the selected user from the XML file
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Status RemoveStudent(Guid Id)
        {
            try
            {
                XmlNode nodeToDelete = data.SelectSingleNode("/Students/Student[@Id='" + Id.ToString() + "']");

                if (nodeToDelete != null)
                {
                    nodeToDelete.ParentNode.RemoveChild(nodeToDelete);
                }
                data.Save(ConfigurationManager.AppSettings["Datasource"]);

                return new Status { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new Status { IsSuccess = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Generic method to deserialize xml to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public T DeserializeToObject<T>(string filepath) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StreamReader sr = new StreamReader(filepath))
            {
                return (T)ser.Deserialize(sr);
            }
        }

    }
}