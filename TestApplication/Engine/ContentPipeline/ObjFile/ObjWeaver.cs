using System;
using System.Collections.Generic;

namespace TestApplication.Engine.ContentPipeline.Obj
{
    public class ObjWeaver
    {
        public (List<float> weavedVertexData, List<int> indexes) WeaveModelData(ModelData modelData)
        {
            List<float> weavedVertexData = new List<float>();
            List<int> indexes = new List<int>();
            Dictionary<string, int> savedIndexes = new Dictionary<string, int>();

            foreach (Face face in modelData.Faces)
            {
                foreach (FacePart facepart in face.FaceParts)
                {
                    if (savedIndexes.ContainsKey(facepart.ToString()))
                    {
                        indexes.Add(savedIndexes[facepart.ToString()]);
                    }
                    else
                    {
                        weavedVertexData.AddRange(modelData.Vertices[facepart.VertexIndex].Range());
                        weavedVertexData.AddRange(modelData.UvCoordinates[facepart.UvIndex].Range());

                        int calculatedIndex = (weavedVertexData.Count / 5) - 1;

                        savedIndexes.Add(facepart.ToString(), calculatedIndex);
                        indexes.Add(calculatedIndex);
                    }
                }
            }

            return (weavedVertexData, indexes);
        }
    }
}