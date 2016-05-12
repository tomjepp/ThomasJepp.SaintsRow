using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow.ClothSimulation.Version05
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

            for (int i = 0; i < Header.NumColliders; i++)
            {
                ClothSimCollisionPrimitiveInfo cscpi = s.ReadStruct<ClothSimCollisionPrimitiveInfo>();
                CollisionPrimitives.Add(cscpi);
            }

            s.Align(8);

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
        }
        
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

            foreach (ClothSimCollisionPrimitiveInfo cscpi in CollisionPrimitives)
            {
                s.WriteStruct(cscpi);
            }

            s.Align(8);

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
        }

        private MiscTypes.FLVector ConvertVector(MiscTypes.SIMDVector128 simdVector)
        {
            MiscTypes.FLVector flVector = new MiscTypes.FLVector();
            flVector.X = simdVector.X;
            flVector.Y = simdVector.Y;
            flVector.Z = simdVector.Z;

            if (simdVector.Z != simdVector.DuplicateZ)
                throw new Exception("simdVector.Z and simdVector.DuplicateZ not equal!");

            return flVector;
        }

        public Version02.ClothSimulationFile ConvertToVersion2()
        {
            Version02.ClothSimulationFile file2 = new Version02.ClothSimulationFile();
            file2.Header.Version = 2;
            file2.Header.DataSize = Header.DataSize;
            file2.Header.Name = Header.Name;
            file2.Header.NumPasses = Header.NumPasses;
            file2.Header.AirResistance = Header.AirResistance;
            file2.Header.WindMultiplier = Header.WindMultiplier;
            file2.Header.WindConst = Header.WindConst;
            file2.Header.GravityMultiplier = Header.GravityMultiplier;
            file2.Header.ObjectVelocityInheritance = Header.ObjectVelocityInheritance;
            file2.Header.ObjectPositionInheritance = Header.ObjectPositionInheritance;
            file2.Header.ObjectRotationInheritance = Header.ObjectRotationInheritance;
            file2.Header.WindType = Header.WindType;
            file2.Header.NumNodes = Header.NumNodes;
            file2.Header.NumAnchorNodes = Header.NumAnchorNodes;
            file2.Header.NumNodeLinks = Header.NumNodeLinks;
            file2.Header.NumRopes = Header.NumRopes;
            file2.Header.NumColliders = Header.NumColliders;
            file2.Header.NodesPtr = (uint)Header.NodesPtr;
            file2.Header.NodeLinksPtr = (uint)Header.NodeLinksPtr;
            file2.Header.RopesPtr = (uint)Header.RopesPtr;
            file2.Header.CollidersPtr = (uint)Header.CollidersPtr;

            foreach (SimulatedNodeInfo sni in Nodes)
            {
                var sni2 = new Version02.SimulatedNodeInfo();
                sni2.BoneIndex = (sbyte)sni.BoneIndex;
                sni2.ParentNodeIndex = (sbyte)sni.ParentNodeIndex;
                sni2.GravityLink = (sbyte)sni.GravityLink;
                sni2.Anchor = (sbyte)sni.Anchor;
                sni2.Collide = sni.Collide;
                sni2.WindMultiplier = sni.WindMultiplier;
                sni2.Pos = ConvertVector(sni.LocalSpacePos);
                sni2.LocalSpacePos = ConvertVector(sni.Pos);

                file2.Nodes.Add(sni2);
            }

            foreach (SimulatedNodeLinkInfo snli in NodeLinks)
            {
                var snli2 = new Version02.SimulatedNodeLinkInfo();
                snli2.NodeIndex1 = snli.NodeIndex1;
                snli2.NodeIndex2 = snli.NodeIndex2;
                snli2.Collide = snli.Collide;
                snli2.Length = snli.Length;
                snli2.StretchLen = snli.StretchLen;
                snli2.Twist = snli.Twist;
                snli2.Spring = snli.Spring;
                snli2.Damp = snli.Damp;

                file2.NodeLinks.Add(snli2);
            }

            foreach (ClothSimRopeInfo csri in Ropes)
            {
                var csri2 = new Version02.ClothSimRopeInfo();
                csri2.Length = csri.Length;
                csri2.NumNodes = csri.NumNodes;
                csri2.NumLinks = csri.NumLinks;
                csri2.NodeIndecies = (uint)csri.NodeIndecies;
                csri2.LinkIndecies = (uint)csri.LinkIndecies;

                file2.Ropes.Add(csri2);
            }

            foreach (List<uint> ropeNodes in RopeNodes)
            {
                List<uint> ropeNodes5 = new List<uint>();
                foreach (uint ropeNode in ropeNodes)
                {
                    ropeNodes5.Add(ropeNode);
                }

                file2.RopeNodes.Add(ropeNodes5);
            }

            foreach (List<uint> ropeLinks in RopeLinks)
            {
                List<uint> ropeLinks5 = new List<uint>();
                foreach (uint ropeLink in ropeLinks)
                {
                    ropeLinks5.Add(ropeLink);
                }

                file2.RopeLinks.Add(ropeLinks5);
            }

            foreach (ClothSimCollisionPrimitiveInfo cscpi in CollisionPrimitives)
            {
                var cscpi2 = new Version02.ClothSimCollisionPrimitiveInfo();
                cscpi2.BoneIndex = cscpi.BoneIndex;
                cscpi2.IsCapsule = cscpi.IsCapsule;
                cscpi2.DoScale = cscpi.DoScale;
                cscpi2.Radius = cscpi.Radius;
                cscpi2.Height = cscpi.Height;
                cscpi2.Pos = ConvertVector(cscpi.Pos);
                cscpi2.Axis = ConvertVector(cscpi.Axis);
                cscpi2.LocalSpacePos = ConvertVector(cscpi.LocalSpacePos);

                file2.CollisionPrimitives.Add(cscpi2);
            }

            return file2;
        }
    }
}
