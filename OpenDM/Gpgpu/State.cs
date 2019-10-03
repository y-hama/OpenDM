using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDM.Gpgpu
{
    public static class State
    {
        #region EventMessage
        public enum EventState
        {
            State,
            Log,
            Option01,
        }

        public class DataEventArgs
        {
            public EventState Mode { get; set; }
            public int Step { get; set; }
            public object Object { get; set; }
        }

        public delegate void UpdateDataEvent(object sender, DataEventArgs e);
        private static UpdateDataEvent UpdateMessageDataEventHandler { get; set; }
        public static event UpdateDataEvent UpdateMessageData
        {
            add { UpdateMessageDataEventHandler += value; }
            remove { UpdateMessageDataEventHandler -= value; }
        }
        public static void SendMessage(EventState state, string message)
        {
            if (UpdateMessageDataEventHandler != null)
            {
                UpdateMessageDataEventHandler(null, new DataEventArgs() { Mode = state, Object = message });
            }
        }

        private static UpdateDataEvent UpdateObjectDataEventHandler { get; set; }
        public static event UpdateDataEvent UpdateObjectData
        {
            add { UpdateObjectDataEventHandler += value; }
            remove { UpdateObjectDataEventHandler -= value; }
        }
        public static void SendData(DataEventArgs e)
        {
            if (UpdateObjectDataEventHandler != null)
            {
                UpdateObjectDataEventHandler(null, e);
            }
        }
        #endregion

        #region GPGPU Define
        public enum MemoryModeSet
        {
            ReadOnly,
            WriteOnly,
            Parameter,
        }
        public enum ActionMode
        {
            Forward,
            Back,
        }
        #endregion

        public static void Initialize()
        {
            Gpgpu_Startup();
        }

        private static void Gpgpu_Startup()
        {
            Core.Instance.UseGPU = true;
            Console.WriteLine(Core.Instance.ProcesserStatus);

            Core.Instance.BuildAllMethod();
        }
    }
}
