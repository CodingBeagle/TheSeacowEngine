using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace TestApplication.Engine.ContentPipeline.Obj
{
    public class ObjLoader
    {
        private readonly string[] fileContent;

        public ObjLoader(string filepath)
        {
            fileContent = File.ReadAllLines(filepath);
        }

        public ModelData LoadModel()
        {
            List<Vertex> vertices = new List<Vertex>();
            List<UvCoordinate> uvCoordinates = new List<UvCoordinate>();
            List<Face> faces = new List<Face>();

            foreach (string line in fileContent)
            {
                var splitUpLine = line.Split(new char[] {' '});

                // Dealing with a vertex
                if (splitUpLine[0] == "v")
                {
                    float vertexX = float.Parse(splitUpLine[1], CultureInfo.InvariantCulture);
                    float vertexY = float.Parse(splitUpLine[2], CultureInfo.InvariantCulture);
                    float vertexZ = float.Parse(splitUpLine[3], CultureInfo.InvariantCulture);

                    vertices.Add(new Vertex(vertexX, vertexY, vertexZ));
                }

                // Dealing with texture coordinates
                if (splitUpLine[0] == "vt")
                {
                    float u = float.Parse(splitUpLine[1], CultureInfo.InvariantCulture);
                    float v = float.Parse(splitUpLine[2], CultureInfo.InvariantCulture);

                    uvCoordinates.Add(new UvCoordinate(u, v));
                }

                // Dealing with face
                if (splitUpLine[0] == "f")
                {
                    // Face parts
                    List<FacePart> faceParts = new List<FacePart>();
                    for (int x = 1; x < 4; x++)
                    {
                        string[] facePartTokens = splitUpLine[x].Split(new char[] { '/' });
                        int vertexIndex = int.Parse(facePartTokens[0]) - 1;
                        int uvCoordinateIndex = int.Parse(facePartTokens[1]) - 1;
                        faceParts.Add(new FacePart(vertexIndex, uvCoordinateIndex));
                    }

                    // Create a face based on this
                    faces.Add(new Face(faceParts));
                }
            }

            return new ModelData(vertices, uvCoordinates, faces);
        }
    }
}