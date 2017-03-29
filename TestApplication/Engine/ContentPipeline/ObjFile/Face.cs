using System.Collections.Generic;

namespace TestApplication.Engine.ContentPipeline.Obj
{
    /// <summary>
    /// 
    /// </summary>
    public class Face
    {
        public List<FacePart> FaceParts { get; }

        public Face(List<FacePart> faceParts)
        {
            FaceParts = faceParts;
        }
    }
}