using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeckleCore;
using System.Threading.Tasks;
using System;

namespace SpeckleUnity
{
    /*
    public class CustomSpeckleUnitySender : SpeckleUnityClient
    {
        public MonoBehaviour monoBehaviour;

        private const string StreamNamePrefix = "UnityStream_";
        /// <summary>
        /// A list containing all the objects for the stream AFTER they had been converted into Speckle objects.
        /// </summary>
        protected List<object> serializedStreamObjects = new List<object>();

        /// <summary>
        /// Key value pairs of Unity <c>GameObject</c>s and SpeckleCore <c>SpeckleObject</c>s to help with 
        /// looking up the corresponding object data to the objects rendered in the scene.
        /// </summary>
        internal Dictionary<GameObject, SpeckleObject> speckleObjectLookup = new Dictionary<GameObject, SpeckleObject>();

        public List<float> numbers = new List<float>();

        protected List<SpeckleUnityObject> nativeObjectsToSend = new List<SpeckleUnityObject>();


        public CustomSpeckleUnitySender ()
        {
            this.streamID = Guid.NewGuid().ToString();
        }
 
        public override async Task InitializeClient(SpeckleUnityManager manager, string url, string authToken)
        {
            this.manager = manager;

            client = new SpeckleApiClient(url.Trim(), true);
            client.BaseUrl = url.Trim();
            RegisterClient();

            await client.IntializeSender(authToken, Application.productName, "Unity", streamID);

            Debug.Log("Initialized sender steam: " + streamID);


            //wait for receiver to be connected
            while (!client.IsConnected) yield return null;

            streamID = client.Stream.StreamId;
            client.Stream.Name = StreamNamePrefix;

            
            //after connected, call update global to get geometry
            await UpdateGlobal();

        }

        protected virtual async Task UpdateGlobal()
        {
            Debug.Log("Update global entered");

            // clone stream
            // why do we need to clone the stream? 
            var cloneResult = client.StreamCloneAsync(streamID);

            // wait until clone is not ready
            while (!cloneResult.IsCompleted) yield return null;
            // add children co cloned stream 
            client.Stream.Children.Add(cloneResult.Result.Clone.StreamId);

            // create speckle objects from native objects
            var convertedObjects = new List<SpeckleObject>();

            foreach (SpeckleUnityObject obj in nativeObjectsToSend)
            {
                var convertedObject = Converter.Serialise(obj) as SpeckleObject;
                convertedObjects.Add(convertedObject);
            }

            var payload = convertedObjects;

            // create new speckle layer - read about layers
            Layer newLayer = new SpeckleCore.Layer()
            {
                Name = "UnityLayer",
                Guid = "TestGUID",
                ObjectCount = convertedObjects.Count,
                StartIndex = 0,
                OrderIndex = 0,
                Properties = new LayerProperties()
                {
                    Color = new SpeckleCore.SpeckleBaseColor()
                    {
                        A = 1,
                        Hex = "Black"
                    },
                },
                Topology = "0-" + convertedObjects.Count.ToString() + " "
            };

            // update/create objects in client
            LocalContext.PruneExistingObjects(convertedObjects, client.BaseUrl);

        }
        
    }
    */

}
