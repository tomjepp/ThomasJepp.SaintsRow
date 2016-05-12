using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.ClothSimulation.Version02
{
    public class ClothSimulationFile
    {
        public ClothSimulationHeader Header;
        public List<SimulatedNodeInfo> Nodes;
        public List<SimulatedNodeLinkInfo> NodeLinks;
        public List<ClothSimRopeInfo> Ropes;
        public List<List<UInt32>> RopeNodes;
        public List<List<UInt32>> RopeLinks;
        public List<ClothSimCollisionPrimitiveInfo> CollisionPrimitives;

        public ClothSimulationFile()
        {
            Header = new ClothSimulationHeader();
            Nodes = new List<SimulatedNodeInfo>();
            NodeLinks = new List<SimulatedNodeLinkInfo>();
            Ropes = new List<ClothSimRopeInfo>();
            RopeNodes = new List<List<uint>>();
            RopeLinks = new List<List<uint>>();
            CollisionPrimitives = new List<ClothSimCollisionPrimitiveInfo>();
        }

        public ClothSimulationFile(Stream s)
        {
            Header = s.ReadStruct<ClothSimulationHeader>();

            Nodes = new List<SimulatedNodeInfo>();
            NodeLinks = new List<SimulatedNodeLinkInfo>();
            Ropes = new List<ClothSimRopeInfo>();
            RopeNodes = new List<List<uint>>();
            RopeLinks = new List<List<uint>>();
            CollisionPrimitives = new List<ClothSimCollisionPrimitiveInfo>();

            for (int i = 0; i < Header.NumNodes; i++)
            {
                SimulatedNodeInfo sni = s.ReadStruct<SimulatedNodeInfo>();
                Nodes.Add(sni);
            }

            for (int i = 0; i < Header.NumNodeLinks; i++)
            {
                SimulatedNodeLinkInfo snli = s.ReadStruct<SimulatedNodeLinkInfo>();
                NodeLinks.Add(snli);
            }

            for (int i = 0; i < Header.NumRopes; i++)
            {
                ClothSimRopeInfo csri = s.ReadStruct<ClothSimRopeInfo>();
                Ropes.Add(csri);
            }

            for (int i = 0; i < Header.NumRopes; i++)
            {
                ClothSimRopeInfo csri = Ropes[i];
                List<UInt32> ropeNodes = new List<uint>();
                
                for (int j = 0; j < csri.NumNodes; j++)
                {
                    uint ropeNode = s.ReadUInt32();
                    ropeNodes.Add(ropeNode);
                }

                RopeNodes.Add(ropeNodes);

                List<UInt32> ropeLinks = new List<uint>();

                for (int j = 0; j < csri.NumLinks; j++)
                {
                    uint ropeLink = s.ReadUInt32();
                    ropeLinks.Add(ropeLink);
                }

                RopeLinks.Add(ropeLinks);
            }

            for (int i = 0; i < Header.NumColliders; i++)
            {
                ClothSimCollisionPrimitiveInfo cscpi = s.ReadStruct<ClothSimCollisionPrimitiveInfo>();
                CollisionPrimitives.Add(cscpi);
            }
        }

        public void Save(Stream s)
        {
            s.WriteStruct(Header);

            foreach (SimulatedNodeInfo sni in Nodes)
            {
                s.WriteStruct(sni);
            }

            foreach (SimulatedNodeLinkInfo snli in NodeLinks)
            {
                s.WriteStruct(snli);
            }

            foreach (ClothSimRopeInfo csri in Ropes)
            {
                s.WriteStruct(csri);
            }

            for (int i = 0; i < Header.NumRopes; i++)
            {
                List<UInt32> ropeNodes = RopeNodes[i];

                foreach (UInt32 ropeNode in ropeNodes)
                {
                    s.WriteUInt32(ropeNode);
                }

                List<UInt32> ropeLinks = RopeLinks[i];

                foreach (UInt32 ropeLink in ropeLinks)
                {
                    s.WriteUInt32(ropeLink);
                }
            }

            foreach (ClothSimCollisionPrimitiveInfo cscpi in CollisionPrimitives)
            {
                s.WriteStruct(cscpi);
            }
        }

        private MiscTypes.SIMDVector128 ConvertVector(MiscTypes.FLVector flVector)
        {
            MiscTypes.SIMDVector128 simdVector = new MiscTypes.SIMDVector128();
            simdVector.X = flVector.X;
            simdVector.Y = flVector.Y;
            simdVector.Z = flVector.Z;
            simdVector.DuplicateZ = flVector.Z;

            return simdVector;
        }

        public Version05.ClothSimulationFile ConvertToVersion5()
        {
            Version05.ClothSimulationFile file5 = new Version05.ClothSimulationFile();
            file5.Header.Version = 5;
            file5.Header.DataSize = Header.DataSize;
            file5.Header.Name = Header.Name;
            file5.Header.NumPasses = Header.NumPasses;
            file5.Header.AirResistance = Header.AirResistance;
            file5.Header.WindMultiplier = Header.WindMultiplier;
            file5.Header.WindConst = Header.WindConst;
            file5.Header.GravityMultiplier = Header.GravityMultiplier;
            file5.Header.ObjectVelocityInheritance = Header.ObjectVelocityInheritance;
            file5.Header.ObjectPositionInheritance = Header.ObjectPositionInheritance;
            file5.Header.ObjectRotationInheritance = Header.ObjectRotationInheritance;
            file5.Header.WindType = Header.WindType;
            file5.Header.NumNodes = Header.NumNodes;
            file5.Header.NumAnchorNodes = Header.NumAnchorNodes;
            file5.Header.NumNodeLinks = Header.NumNodeLinks;
            file5.Header.NumRopes = Header.NumRopes;
            file5.Header.NumColliders = Header.NumColliders;
            file5.Header.BoundingSphereRadius = 1f; // Check this value!
            file5.Header.NodesPtr = Header.NodesPtr;
            file5.Header.NodeLinksPtr = Header.NodeLinksPtr;
            file5.Header.RopesPtr = Header.RopesPtr;
            file5.Header.CollidersPtr = Header.CollidersPtr;

            foreach (SimulatedNodeInfo sni2 in Nodes)
            {
                var sni5 = new Version05.SimulatedNodeInfo();
                sni5.BoneIndex = sni2.BoneIndex;
                sni5.ParentNodeIndex = sni2.ParentNodeIndex;
                sni5.GravityLink = sni2.GravityLink;
                sni5.Anchor = sni2.Anchor;
                sni5.Collide = sni2.Collide;
                sni5.WindMultiplier = sni2.WindMultiplier;
                sni5.Pos = ConvertVector(sni2.LocalSpacePos);
                sni5.LocalSpacePos = ConvertVector(sni2.Pos);

                file5.Nodes.Add(sni5);
            }

            foreach (SimulatedNodeLinkInfo snli2 in NodeLinks)
            {
                var snli5 = new Version05.SimulatedNodeLinkInfo();
                snli5.NodeIndex1 = snli2.NodeIndex1;
                snli5.NodeIndex2 = snli2.NodeIndex2;
                snli5.Collide = snli2.Collide;
                snli5.Length = snli2.Length;
                snli5.StretchLen = snli2.StretchLen;
                snli5.Twist = snli2.Twist;
                snli5.Spring = snli2.Spring;
                snli5.Damp = snli2.Damp;
                snli5.IsGravityLink = true;

                file5.NodeLinks.Add(snli5);
            }

            foreach (ClothSimRopeInfo csri2 in Ropes)
            {
                var csri5 = new Version05.ClothSimRopeInfo();
                csri5.Length = csri2.Length;
                csri5.NumNodes = csri2.NumNodes;
                csri5.NumLinks = csri2.NumLinks;
                csri5.NodeIndecies = csri2.NodeIndecies;
                csri5.LinkIndecies = csri2.LinkIndecies;

                file5.Ropes.Add(csri5);
            }

            foreach (List<uint> ropeNodes in RopeNodes)
            {
                List<uint> ropeNodes5 = new List<uint>();
                foreach (uint ropeNode in ropeNodes)
                {
                    ropeNodes5.Add(ropeNode);
                }

                file5.RopeNodes.Add(ropeNodes5);
            }

            foreach (List<uint> ropeLinks in RopeLinks)
            {
                List<uint> ropeLinks5 = new List<uint>();
                foreach (uint ropeLink in ropeLinks)
                {
                    ropeLinks5.Add(ropeLink);
                }

                file5.RopeLinks.Add(ropeLinks5);
            }

            foreach (ClothSimCollisionPrimitiveInfo cscpi in CollisionPrimitives)
            {
                var cscpi5 = new Version05.ClothSimCollisionPrimitiveInfo();
                cscpi5.BoneIndex = cscpi.BoneIndex;
                cscpi5.IsCapsule = cscpi.IsCapsule;
                cscpi5.DoScale = cscpi.DoScale;
                cscpi5.Radius = cscpi.Radius;
                cscpi5.Height = cscpi.Height;
                cscpi5.Pos = ConvertVector(cscpi.Pos);
                cscpi5.Axis = ConvertVector(cscpi.Axis);
                cscpi5.LocalSpacePos = ConvertVector(cscpi.LocalSpacePos);

                file5.CollisionPrimitives.Add(cscpi5);
            }

            return file5;
        }
    }
}
