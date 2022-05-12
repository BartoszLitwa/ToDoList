using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Domain.Api.V1
{
    public static class Routes
    {
        private const string _API = "api/";

        public static class V1
        {
            private const string _VERSION = "v1/";
            private const string _BASE = _API + _VERSION;
            public const string SWAGGER = _VERSION + "swagger.json";

            public static class Identity
            {
                public const string CONTROLLER = _BASE + "identity/";

                public const string LOGIN = CONTROLLER + "login";
                public const string LOGOUT = CONTROLLER + "logout";
                public const string REGISTER = CONTROLLER + "register";
                public const string REFRESH = CONTROLLER + "refresh";
            }

            public static class TaskList
            {
                public const string CONTROLLER = _BASE + "taskList/";

                public const string CREATE = CONTROLLER + "create";
                public const string LIST = CONTROLLER + "get";
                public const string GET = CONTROLLER + "get/{id}";
                public const string UPDATE = CONTROLLER + "update";
                public const string DELETE = CONTROLLER + "delete";
            }

            public static class Task
            {
                public const string CONTROLLER = _BASE + "task/";

                public const string GET = CONTROLLER + "get/{id}";
                public const string GET_TASK = CONTROLLER + "getTask";
                public const string CREATE = CONTROLLER + "create";
                public const string UPDATE = CONTROLLER + "update";
                public const string DELETE = CONTROLLER + "delete";
                public const string TOGGLE = CONTROLLER + "toggle";
                public const string TOGGLE_BY_ID = CONTROLLER + "toggle/{taskId}";
            }

            public static class Account
            {
                public const string CONTROLLER = _BASE + "account/";

                public const string GET = CONTROLLER + "get";
                public const string GET_USER = CONTROLLER + "getuser/{userId}";
                public const string CHANGE_USERNAME = CONTROLLER + "username";
                public const string CHANGE_EMAIL = CONTROLLER + "email";
                public const string CHANGE_PASSWORD = CONTROLLER + "password";
                public const string DELETE = CONTROLLER + "delete";
            }
        }
    }
}
