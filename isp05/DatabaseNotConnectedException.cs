using System;

namespace isp05
{
    public class DatabaseNotConnectedException : Exception
    {
        public DatabaseNotConnectedException()
        {
            
        }

        public DatabaseNotConnectedException(string message) : base(message)
        {
            
        }
        
        public DatabaseNotConnectedException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}