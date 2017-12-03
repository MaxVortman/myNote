using System.ComponentModel;

namespace myNote.ClientService
{
    public class Api
    {
        #region Private properties

        private static Api instance;
        private UserService _userService;
        private ShareService _shareService;
        private NoteService _noteService;
        private NoteGroupService _noteGroupService;
        private LoginService _loginService;
        private GroupService _groupService;

        #endregion
        
        #region Default private constructor

        private Api(string connectionString)
        {
            _userService = new UserService(connectionString);
            _noteGroupService = new NoteGroupService(connectionString);
            _noteService = new NoteService(connectionString);
            _groupService = new GroupService(connectionString);
            _shareService = new ShareService(connectionString);
            _loginService = new LoginService(connectionString);
        }

        #endregion

        #region Create Instance

        public static Api CreateInstance(string connectionString)
        {
            if(instance == null)
                instance = new Api(connectionString);
            return instance;
        }

        #endregion

        #region Service methods

        /// <summary>
        /// Get User's api service 
        /// </summary>
        /// <returns></returns>
        public UserService GetUserService()
        {
            return _userService;
        }
        /// <summary>
        /// Get Note api service
        /// </summary>
        /// <returns></returns>
        public NoteService GetNoteService()
        {
            return _noteService;
        }
        /// <summary>
        /// Get group api service
        /// </summary>
        /// <returns></returns>
        public GroupService GetGroupService()
        {
            return _groupService;
        }
        /// <summary>
        /// Get login api service
        /// </summary>
        /// <returns></returns>
        public LoginService GetLoginService()
        {
            return _loginService;
        }
        /// <summary>
        /// Get note group api service
        /// </summary>
        /// <returns></returns>
        public NoteGroupService GetNoteGroupService()
        {
            return _noteGroupService;
        }
        /// <summary>
        /// Get share api service
        /// </summary>
        /// <returns></returns>
        public ShareService GetShareService()
        {
            return _shareService;
        }

        #endregion
    }
}