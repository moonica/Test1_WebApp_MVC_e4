using Test1_WebApp_MVC.Models;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Test1_WebApp_MVC.DAL
{
    public class XmlUserDataContext : IUserDataContext
    {
        private string _filePath = @"DAL/Users.xml"; //TODO: store in config
        private ILogger _logger;
        
        public string ErrorMessage = string.Empty;

        /// <summary>
        /// Construct a new instance of XmlUserDataContext, using the preconfigured file path
        /// </summary>
        /// <param name="logger">Logging mechanism</param>
        public XmlUserDataContext(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Construct a new instance of XmlUserDataContext
        /// </summary>
        /// <param name="filePath">Specify relative path to XML file</param>
        /// <param name="logger">Logging mechanism</param>
        public XmlUserDataContext(string filePath,  ILogger logger)
        {
            _filePath = filePath;
            _logger = logger;
        }

        #region PUBLIC METHODS

        /// <summary>
        /// Add a new user node to the end of the ArrayOfUsers node in the XML file
        /// </summary>
        public bool CreateUser(User newUser)
        {
            try
            {
                if (!validateUser(newUser))
                    return false;

                var nextId = (getMaxId() ?? -1) + 1;

                XDocument xmlFile = XDocument.Load(_filePath);
                newUser.Id = nextId;
                var newElem = getXElementFromUser(newUser);

                xmlFile?.Element("ArrayOfUser")?.LastNode?.AddAfterSelf(newElem);

                xmlFile.Save(_filePath);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Update an existing user with the given properties. Blank fields will overwrite existing fields
        /// </summary>
        public bool UpdateUser(User existingUser)
        {
            if ((existingUser is null) || (existingUser.Id <= 0))
                return false;

            try
            {
                XDocument xmlFile = XDocument.Load(_filePath);

                var elemToUpdate = xmlFile
                    .Elements("ArrayOfUser")
                    .Elements("User")
                    .Where(elem => elem.Elements("Id").Any(i => i.Value == existingUser?.Id.ToString()))
                    .FirstOrDefault();

                if (elemToUpdate is null)
                    return false;

                var newElem = getXElementFromUser(existingUser);//, elemToUpdate);
                //enable the second parameter to coalesce, ie not overwrite with blanks

                elemToUpdate.ReplaceWith(newElem);

                xmlFile.Save(_filePath);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Delete a user node from the XML path using the given ID
        /// </summary>
        public bool DeleteUser(int userIdToDelete)
        {
            try
            {
                XDocument xmlFile = XDocument.Load(_filePath);

                var elemToDelete = xmlFile.Elements("ArrayOfUser")
                    .Elements("User")
                    .Where(elem => elem.Elements("Id").Any(i => i.Value == userIdToDelete.ToString()));

                if (elemToDelete == null)
                    return false;

                elemToDelete.Remove();                            

                xmlFile.Save(_filePath);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Get a specific user by ID
        /// </summary>
        /// <param name="userId"></param>
        public User GetUser(int userId)
        {
            //try
            //{
            //    return new User();
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.Message);
            //    return null;
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all users from the XML file as a list of User objects
        /// </summary>        
        public List<User> GetUsers()
        {
            try
            {
                var result = new List<User>();

                var serializer = new XmlSerializer(typeof(User[]));
                using (var reader = new StreamReader(_filePath))
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

        private int? getMaxId()
        {
            var users = GetUsers();
            return users?.MaxBy(u => u.Id)?.Id;
        }

        private bool validateUser(User newUser)
        {
            return newUser?.IsValid() ?? false;
        }

        private bool createFirstUser(User newUser)
        {
            try
            {
                if (!validateUser(newUser))
                    return false;

                var serializer = new XmlSerializer(typeof(User));
                using (var writer = new StreamWriter(_filePath))
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

        /// <summary>
        /// Converts a "User" XML element to a User object. If an existing element is provided, the values (except ID) are coalsesced (the new value is used if present, else the existing value is defaulted to)
        /// </summary>
        /// <param name="userNewValues"></param>
        /// <param name="existingElement"></param>
        /// <returns></returns>
        private XElement getXElementFromUser(User userNewValues, XElement existingElement = null)
        {
            var existingUser = getUserFromXElement(existingElement);

            return new XElement("User",
                new XElement(nameof(userNewValues.Id), userNewValues.Id),
                new XElement(nameof(userNewValues.Name), coalesce(userNewValues.Name, existingUser?.Name)),
                new XElement(nameof(userNewValues.Surname), coalesce(userNewValues.Surname, existingUser?.Surname)),
                new XElement(nameof(userNewValues.CellNumber), coalesce(userNewValues.CellNumber, existingUser?.CellNumber))
            );
        }

        private string coalesce(string preferredValue, string fallbackValue)
        {
            return (string.IsNullOrEmpty(preferredValue?.Trim()) ? (fallbackValue ?? string.Empty) : preferredValue);
        }

        private User getUserFromXElement(XElement existingElement)
        {
            var result = new User();

            if (existingElement is null)
               return null;

            result.Name = existingElement.Element(nameof(result.Name)).Value;
            result.Surname = existingElement.Element(nameof(result.Surname)).Value;
            result.CellNumber = existingElement.Element(nameof(result.CellNumber)).Value;

            return result;
        }

        #endregion PRIVATE METHODS
    }
}