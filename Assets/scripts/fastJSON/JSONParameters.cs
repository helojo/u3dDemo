namespace fastJSON
{
    using System;

    public sealed class JSONParameters
    {
        public bool EnableAnonymousTypes;
        public bool IgnoreCaseOnDeserialize;
        public bool SerializeNullValues = true;
        public bool ShowReadOnlyProperties;
        public bool UseEscapedUnicode = true;
        public bool UseExtensions = true;
        public bool UseFastGuid = true;
        public bool UseOptimizedDatasetSchema = true;
        public bool UseUTCDateTime = true;
        public bool UsingGlobalTypes = true;

        public void FixValues()
        {
            if (!this.UseExtensions)
            {
                this.UsingGlobalTypes = false;
            }
        }
    }
}

