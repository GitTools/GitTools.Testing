namespace GitTools.Testing.Internal
{
    using System;
    using System.IO;
    using System.Reflection;

    static class PathHelper
    {
        public static string GetTempPath()
        {
            return Path.Combine(GetCurrentDirectory(), "TestRepositories", Guid.NewGuid().ToString());
        }

        private static string GetCurrentDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}