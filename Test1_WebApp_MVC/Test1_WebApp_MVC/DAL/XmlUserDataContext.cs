﻿using Test1_WebApp_MVC.Models;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Test1_WebApp_MVC.DAL
{
    public class XmlUserDataContext : IUserDataContext
    {
        private readonly string _fileName = @"DAL/Users.xml";
        private ILogger _logger;

        public XmlUserDataContext(ILogger logger)
        {
            _logger = logger;
        }

        #region PUBLIC METHODS

        public bool CreateUser(User newUser)
        {
            try
            {
                if (!validateUser(newUser))
                    return false;

                var nextId = (getMaxId() ?? -1) + 1;

                XDocument xmlFile = XDocument.Load(_fileName);
                var newElem = getUserXElement(newUser, nextId);

                xmlFile?.Element("ArrayOfUser")?.LastNode?.AddAfterSelf(newElem);

                xmlFile.Save(_fileName);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public bool UpdateUser(User existingUser)
        {
            //try
            //{
            //    if (!validateUser(existingUser))
            //        return false;

            //    XDocument xmlFile = XDocument.Load(_fileName);
            //    var query = from c in xmlFile.Elements("ArrayOfUser").Elements("User").Where(x => x.Attribute("Id")?.ToString() == existingUser.Id.ToString())
            //                select c;
            //    foreach (XElement userNode in query)
            //    {
            //        if (userNode == null)
            //            continue;

            //        populateNode(userNode, existingUser, true);
            //    }
            //    xmlFile.Save(_fileName);
                return true;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.Message);
            //    return false;
            //}
        }

        public bool DeleteUser(int userIdToDelete)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public User GetUser(int userId)
        {
            try
            {
                return new User();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public List<User> GetUsers()
        {
            try
            {
                var result = new List<User>();

                var serializer = new XmlSerializer(typeof(User[]));
                using (var reader = new StreamReader(_fileName))
                {
                    var xmlResult = serializer.Deserialize(reader);

                    if (xmlResult?.GetType() == typeof(User[]))
                    {
                        var userArray = (User[])xmlResult;
                        result = userArray.ToList();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        #endregion PUBLIC METHODS


        #region PRIVATE METHODS

        private int? getMaxId(IEnumerable<XElement> query)
        {
            var idStrElements = query?.Select(x => x?.Element("Id")?.Value ?? "-1");
            var idElements = new List<int>();
            idStrElements?.ToList()?.ForEach(e =>
            {
                if (int.TryParse(e, out int id))
                    idElements.Add(id);
            });

            return idElements?.Max();
        }

        private int? getMaxId()
        {
            var users = GetUsers();
            return users?.MaxBy(u => u.Id)?.Id;
        }

        private bool validateUser(User newUser)
        {
            return true;
        }

        private bool createFirstUser(User newUser)
        {
            try
            {
                if (!validateUser(newUser))
                    return false;

                var serializer = new XmlSerializer(typeof(User));
                using (var writer = new StreamWriter(_fileName))
                {
                    serializer.Serialize(writer, newUser);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        private void populateNode(XElement userNode, User newUser, bool coalesce = false)
        {
            if ((userNode == null) || (newUser == null))
                return;

            populateNodeAttribute(userNode, newUser.Name, "Name", coalesce);
            populateNodeAttribute(userNode, newUser.Surname, "Surname", coalesce);
            populateNodeAttribute(userNode, newUser.CellNumber, "CellNumber", coalesce);
        }

        private void populateNodeAttribute(XElement userNode, string newValue, string attrName, bool coalesce = false)
        {
            userNode.Attribute(attrName).Value = coalesce ? (newValue ?? userNode.Attribute(attrName).Value) : newValue;
        }

        private XElement getUserXElement(User newUser, int id)
        {
            return new XElement("User",
                new XElement(nameof(newUser.Id), id),
                new XElement(nameof(newUser.Name), newUser.Name),
                new XElement(nameof(newUser.Surname), newUser.Surname),
                new XElement(nameof(newUser.CellNumber), newUser.CellNumber)
            );
        }

        #endregion PRIVATE METHODS
    }
}