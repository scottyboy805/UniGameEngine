﻿
namespace UniGameEngine
{
    public interface IContentCallback
    {
        // Methods
        void OnBeforeContentSave();

        void OnAfterContentLoad();
    }
}
