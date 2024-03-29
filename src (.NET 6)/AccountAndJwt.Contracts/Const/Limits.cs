﻿namespace AccountAndJwt.Contracts.Const
{
    public static class Limits
    {
        public class Value
        {
            public const Int32 CommentaryMaxLength = 500;
        }
        public class User
        {
            public const Int32 LoginMaxLength = 50;
            public const Int32 FirstNameMaxLength = 50;
            public const Int32 LastNameMaxLength = 50;
        }
        public class Password
        {
            public const Int32 MinLength = 5;
            public const Int32 MaxLength = 30;
        }
    }
}