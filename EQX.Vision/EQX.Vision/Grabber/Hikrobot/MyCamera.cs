using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace EQX.Vision.Grabber
{
    public class MyCamera
    {
        #region ????
        /// <summary>
        /// Grab callback
        /// </summary>
        /// <param name="pData">Image data</param>
        /// <param name="pFrameInfo">Frame info</param>
        /// <param name="pUser">User defined variable</param>
        public delegate void cbOutputdelegate(IntPtr pData, ref MV_FRAME_OUT_INFO pFrameInfo, IntPtr pUser);

        /// <summary>
        /// Grab callback
        /// </summary>
        /// <param name="pData">Image data</param>
        /// <param name="pFrameInfo">Frame info</param>
        /// <param name="pUser">User defined variable</param>
        public delegate void cbOutputExdelegate(IntPtr pData, ref MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser);

        /// <summary>
        /// Xml Update callback(Interfaces not recommended)
        /// </summary>
        /// <param name="enType">Node type</param>
        /// <param name="pstFeature">Current node feature structure</param>
        /// <param name="pstNodesList">Nodes list</param>
        /// <param name="pUser">User defined variable</param>
        public delegate void cbXmlUpdatedelegate(MV_XML_InterfaceType enType, IntPtr pstFeature, ref MV_XML_NODES_LIST pstNodesList, IntPtr pUser);

        /// <summary>
        /// Exception callback
        /// </summary>
        /// <param name="nMsgType">Msg type</param>
        /// <param name="pUser">User defined variable</param>
        public delegate void cbExceptiondelegate(UInt32 nMsgType, IntPtr pUser);

        /// <summary>
        /// Event callback (Interfaces not recommended)
        /// </summary>
        /// <param name="nUserDefinedId">User defined ID</param>
        /// <param name="pUser">User defined variable</param>
        public delegate void cbEventdelegate(UInt32 nUserDefinedId, IntPtr pUser);

        /// <summary>
        /// Event callback
        /// </summary>
        /// <param name="pEventInfo">Event Info</param>
        /// <param name="pUser">User defined variable</param>
        public delegate void cbEventdelegateEx(ref MV_EVENT_OUT_INFO pEventInfo, IntPtr pUser);
        #endregion

        #region ??????????
        /// <summary>
        /// Get SDK Version
        /// </summary>
        /// <returns>Always return 4 Bytes of version number |Main  |Sub   |Rev   |Test|
        ///                                                   8bits  8bits  8bits  8bits 
        /// </returns>
        public static UInt32 MV_CC_GetSDKVersion_NET()
        {
            return MV_CC_GetSDKVersion();
        }

        /// <summary>
        /// Get supported Transport Layer
        /// </summary>
        /// <returns>Supported Transport Layer number</returns>
        public static Int32 MV_CC_EnumerateTls_NET()
        {
            return MV_CC_EnumerateTls();
        }

        /// <summary>
        /// Enumerate Device
        /// </summary>
        /// <param name="nTLayerType">Enumerate TLs</param>
        /// <param name="stDevList">Device List</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public static Int32 MV_CC_EnumDevices_NET(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList)
        {
            return MV_CC_EnumDevices(nTLayerType, ref stDevList);
        }

        /// <summary>
        /// Enumerate device according to manufacture name
        /// </summary>
        /// <param name="nTLayerType">Enumerate TLs</param>
        /// <param name="stDevList">Device List</param>
        /// <param name="pManufacturerName">Manufacture Name</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public static Int32 MV_CC_EnumDevicesEx_NET(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList, string pManufacturerName)
        {
            return MV_CC_EnumDevicesEx(nTLayerType, ref stDevList, pManufacturerName);
        }

        /// <summary>
        /// Is the device accessible
        /// </summary>
        /// <param name="stDevInfo">Device Information</param>
        /// <param name="nAccessMode">Access Right</param>
        /// <returns>Access, return true. Not access, return false</returns>
        public static Boolean MV_CC_IsDeviceAccessible_NET(ref MV_CC_DEVICE_INFO stDevInfo, UInt32 nAccessMode)
        {
            return MV_CC_IsDeviceAccessible(ref stDevInfo, nAccessMode);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MyCamera()
        {
            handle = IntPtr.Zero;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~MyCamera()
        {
            //MV_CC_DestroyDevice_NET();
        }

        /// <summary>
        /// Create Device
        /// </summary>
        /// <param name="stDevInfo">Device Information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_CreateDevice_NET(ref MV_CC_DEVICE_INFO stDevInfo)
        {
            if (IntPtr.Zero != handle)
            {
                MV_CC_DestroyHandle(handle);
                handle = IntPtr.Zero;
            }

            return MV_CC_CreateHandle(ref handle, ref stDevInfo);
        }

        /// <summary>
        /// Create Device without log
        /// </summary>
        /// <param name="stDevInfo">Device Information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_CreateDeviceWithoutLog_NET(ref MV_CC_DEVICE_INFO stDevInfo)
        {
            if (IntPtr.Zero != handle)
            {
                MV_CC_DestroyHandle(handle);
                handle = IntPtr.Zero;
            }

            return MV_CC_CreateHandleWithoutLog(ref handle, ref stDevInfo);
        }

        /// <summary>
        /// Destroy Device
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_DestroyDevice_NET()
        {
            Int32 nRet = MV_CC_DestroyHandle(handle);
            handle = IntPtr.Zero;
            return nRet;
        }

        /// <summary>
        /// Open Device
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_OpenDevice_NET()
        {
            return MV_CC_OpenDevice(handle, 1, 0);
        }

        /// <summary>
        /// Open Device
        /// </summary>
        /// <param name="nAccessMode">Access Right</param>
        /// <param name="nSwitchoverKey">Switch key of access right</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_OpenDevice_NET(UInt32 nAccessMode, UInt16 nSwitchoverKey)
        {
            return MV_CC_OpenDevice(handle, nAccessMode, nSwitchoverKey);
        }

        /// <summary>
        /// Close Device
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_CloseDevice_NET()
        {
            return MV_CC_CloseDevice(handle);
        }

        /// <summary>
        /// Is the device connected
        /// </summary>
        /// <returns>Connected, return true. Not Connected or DIsconnected, return false</returns>
        public Boolean MV_CC_IsDeviceConnected_NET()
        {
            return MV_CC_IsDeviceConnected(handle);
        }

        /// <summary>
        /// Register the image callback function
        /// </summary>
        /// <param name="cbOutput">Callback function pointer</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_RegisterImageCallBackEx_NET(cbOutputExdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBackEx(handle, cbOutput, pUser);
        }

        /// <summary>
        /// Register the RGB image callback function
        /// </summary>
        /// <param name="cbOutput">Callback function pointer</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_RegisterImageCallBackForRGB_NET(cbOutputExdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBackForRGB(handle, cbOutput, pUser);
        }

        /// <summary>
        /// Register the BGR image callback function
        /// </summary>
        /// <param name="cbOutput">Callback function pointer</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_RegisterImageCallBackForBGR_NET(cbOutputExdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBackForBGR(handle, cbOutput, pUser);
        }

        /// <summary>
        /// Start Grabbing
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_StartGrabbing_NET()
        {
            return MV_CC_StartGrabbing(handle);
        }

        /// <summary>
        /// Stop Grabbing
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_StopGrabbing_NET()
        {
            return MV_CC_StopGrabbing(handle);
        }

        /// <summary>
        /// Get one frame of RGB image, this function is using query to get data
        /// query whether the internal cache has data, get data if there has, return error code if no data
        /// </summary>
        /// <param name="pData">Image data receiving buffer</param>
        /// <param name="nDataSize">Buffer size</param>
        /// <param name="pFrameInfo">Image information</param>
        /// <param name="nMsec">Waiting timeout</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetImageForRGB_NET(IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec)
        {
            return MV_CC_GetImageForRGB(handle, pData, nDataSize, ref pFrameInfo, nMsec);
        }

        /// <summary>
        /// Get one frame of BGR image, this function is using query to get data
        /// query whether the internal cache has data, get data if there has, return error code if no data
        /// </summary>
        /// <param name="pData">Image data receiving buffer</param>
        /// <param name="nDataSize">Buffer size</param>
        /// <param name="pFrameInfo">Image information</param>
        /// <param name="nMsec">Waiting timeout</param>
        /// <returns>Success, return MV_OK. Failure, return error cod</returns>
        public Int32 MV_CC_GetImageForBGR_NET(IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec)
        {
            return MV_CC_GetImageForBGR(handle, pData, nDataSize, ref pFrameInfo, nMsec);
        }

        /// <summary>
        /// Get a frame of an image using an internal cache
        /// </summary>
        /// <param name="pFrame">Image data and image information</param>
        /// <param name="nMsec">Waiting timeout</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetImageBuffer_NET(ref MV_FRAME_OUT pFrame, Int32 nMsec)
        {
            return MV_CC_GetImageBuffer(handle, ref pFrame, nMsec);
        }

        /// <summary>
        /// Free image buffer(used with MV_CC_GetImageBuffer)
        /// </summary>
        /// <param name="pFrame">Image data and image information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_FreeImageBuffer_NET(ref MV_FRAME_OUT pFrame)
        {
            return MV_CC_FreeImageBuffer(handle, ref pFrame);
        }

        /// <summary>
        /// Get a frame of an image
        /// </summary>
        /// <param name="pData">Image data receiving buffer</param>
        /// <param name="nDataSize">Buffer size</param>
        /// <param name="pFrameInfo">Image information</param>
        /// <param name="nMsec">Waiting timeout</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetOneFrameTimeout_NET(IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec)
        {
            return MV_CC_GetOneFrameTimeout(handle, pData, nDataSize, ref pFrameInfo, nMsec);
        }

        /// <summary>
        /// Clear image Buffers to clear old data
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_ClearImageBuffer_NET()
        {
            return MV_CC_ClearImageBuffer(handle);
        }

        /// <summary>
        /// Display one frame image
        /// </summary>
        /// <param name="pDisplayInfo">Image information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_DisplayOneFrame_NET(ref MV_DISPLAY_FRAME_INFO pDisplayInfo)
        {
            return MV_CC_DisplayOneFrame(handle, ref pDisplayInfo);
        }

        /// <summary>
        /// Set the number of the internal image cache nodes in SDK(Greater than or equal to 1, to be called before the capture)
        /// </summary>
        /// <param name="nNum">Number of cache nodes</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetImageNodeNum_NET(UInt32 nNum)
        {
            return MV_CC_SetImageNodeNum(handle, nNum);
        }

        /// <summary>
        /// Set Grab Strategy
        /// </summary>
        /// <param name="enGrabStrategy">The value of grab strategy</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetGrabStrategy_NET(MV_GRAB_STRATEGY enGrabStrategy)
        {
            return MV_CC_SetGrabStrategy(handle, enGrabStrategy);
        }

        /// <summary>
        /// Set The Size of Output Queue(Only work under the strategy of MV_GrabStrategy_LatestImages,rang:1-ImageNodeNum)
        /// </summary>
        /// <param name="nOutputQueueSize">The Size of Output Queue</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetOutputQueueSize_NET(UInt32 nOutputQueueSize)
        {
            return MV_CC_SetOutputQueueSize(handle, nOutputQueueSize);
        }

        /// <summary>
        /// Get device information(Called before start grabbing)
        /// </summary>
        /// <param name="pstDevInfo">device information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetDeviceInfo_NET(ref MV_CC_DEVICE_INFO pstDevInfo)
        {
            return MV_CC_GetDeviceInfo(handle, ref pstDevInfo);
        }

        /// <summary>
        /// Get various type of information
        /// </summary>
        /// <param name="pstInfo">Various type of information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetAllMatchInfo_NET(ref MV_ALL_MATCH_INFO pstInfo)
        {
            return MV_CC_GetAllMatchInfo(handle, ref pstInfo);
        }
        #endregion

        #region ??????????????
        /// <summary>
        /// Get Integer value
        /// </summary>
        /// <param name="strKey">Key value, for example, using "Width" to get width</param>
        /// <param name="pstValue">Value of device features</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetIntValueEx_NET(String strKey, ref MVCC_INTVALUE_EX pstValue)
        {
            return MV_CC_GetIntValueEx(handle, strKey, ref pstValue);
        }

        /// <summary>
        /// Set Integer value
        /// </summary>
        /// <param name="strKey">Key value, for example, using "Width" to set width</param>
        /// <param name="nValue">Feature value to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetIntValueEx_NET(String strKey, Int64 nValue)
        {
            return MV_CC_SetIntValueEx(handle, strKey, nValue);
        }

        /// <summary>
        /// Get Enum value
        /// </summary>
        /// <param name="strKey">Key value, for example, using "PixelFormat" to get pixel format</param>
        /// <param name="pstValue">Value of device features</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetEnumValue_NET(String strKey, ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetEnumValue(handle, strKey, ref pstValue);
        }

        /// <summary>
        /// Set Enum value
        /// </summary>
        /// <param name="strKey">Key value, for example, using "PixelFormat" to set pixel format</param>
        /// <param name="nValue">Feature value to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetEnumValue_NET(String strKey, UInt32 nValue)
        {
            return MV_CC_SetEnumValue(handle, strKey, nValue);
        }

        /// <summary>
        /// Set Enum value
        /// </summary>
        /// <param name="strKey">Key value, for example, using "PixelFormat" to set pixel format</param>
        /// <param name="sValue">Feature String to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetEnumValueByString_NET(String strKey, String sValue)
        {
            return MV_CC_SetEnumValueByString(handle, strKey, sValue);
        }
        /// <summary>
        /// Get Float value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="pstValue">Value of device features</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetFloatValue_NET(String strKey, ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetFloatValue(handle, strKey, ref pstValue);
        }

        /// <summary>
        /// Set float value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="fValue">Feature value to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetFloatValue_NET(String strKey, Single fValue)
        {
            return MV_CC_SetFloatValue(handle, strKey, fValue);
        }

        /// <summary>
        /// Get Boolean value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="pbValue">Value of device features</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetBoolValue_NET(String strKey, ref Boolean pbValue)
        {
            return MV_CC_GetBoolValue(handle, strKey, ref pbValue);
        }

        /// <summary>
        /// Set Boolean value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="bValue">Feature value to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetBoolValue_NET(String strKey, Boolean bValue)
        {
            return MV_CC_SetBoolValue(handle, strKey, bValue);
        }

        /// <summary>
        /// Get String value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="pstValue">Value of device features</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetStringValue_NET(String strKey, ref MVCC_STRINGVALUE pstValue)
        {
            return MV_CC_GetStringValue(handle, strKey, ref pstValue);
        }

        /// <summary>
        /// Set String value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="strValue">Feature value to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetStringValue_NET(String strKey, String strValue)
        {
            return MV_CC_SetStringValue(handle, strKey, strValue);
        }

        /// <summary>
        /// Send Command
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetCommandValue_NET(String strKey)
        {
            return MV_CC_SetCommandValue(handle, strKey);
        }

        /// <summary>
        /// Invalidate GenICam Nodes
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_InvalidateNodes_NET()
        {
            return MV_CC_InvalidateNodes(handle);
        }
        #endregion

        #region ???? ? ????? ????????
        /// <summary>
        /// Device Local Upgrade
        /// </summary>
        /// <param name="pFilePathName">File path and name</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_LocalUpgrade_NET(String pFilePathName)
        {
            return MV_CC_LocalUpgrade(handle, pFilePathName);
        }

        /// <summary>
        /// Get Upgrade Progress
        /// </summary>
        /// <param name="pnProcess">Value of Progress</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetUpgradeProcess_NET(ref UInt32 pnProcess)
        {
            return MV_CC_GetUpgradeProcess(handle, ref pnProcess);
        }

        /// <summary>
        /// Read Memory
        /// </summary>
        /// <param name="pBuffer">Used as a return value, save the read-in memory value(Memory value is stored in accordance with the big end model)</param>
        /// <param name="nAddress">Memory address to be read, which can be obtained from the Camera.xml file of the device, the form xml node value of xxx_RegAddr</param>
        /// <param name="nLength">Length of the memory to be read</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_ReadMemory_NET(IntPtr pBuffer, Int64 nAddress, Int64 nLength)
        {
            return MV_CC_ReadMemory(handle, pBuffer, nAddress, nLength);
        }

        /// <summary>
        /// Write Memory
        /// </summary>
        /// <param name="pBuffer">Memory value to be written ( Note the memory value to be stored in accordance with the big end model)</param>
        /// <param name="nAddress">Memory address to be written, which can be obtained from the Camera.xml file of the device, the form xml node value of xxx_RegAddr</param>
        /// <param name="nLength">Length of the memory to be written</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_WriteMemory_NET(IntPtr pBuffer, Int64 nAddress, Int64 nLength)
        {
            return MV_CC_WriteMemory(handle, pBuffer, nAddress, nLength);
        }

        /// <summary>
        /// Register Exception Message CallBack, call after open device
        /// </summary>
        /// <param name="cbException">Exception Message CallBack Function</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_RegisterExceptionCallBack_NET(cbExceptiondelegate cbException, IntPtr pUser)
        {
            return MV_CC_RegisterExceptionCallBack(handle, cbException, pUser);
        }

        /// <summary>
        /// Register event callback, which is called after the device is opened
        /// </summary>
        /// <param name="cbEvent">Event CallBack Function</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_RegisterAllEventCallBack_NET(cbEventdelegateEx cbEvent, IntPtr pUser)
        {
            return MV_CC_RegisterAllEventCallBack(handle, cbEvent, pUser);
        }

        /// <summary>
        /// Register single event callback, which is called after the device is opened
        /// </summary>
        /// <param name="pEventName">Event name</param>
        /// <param name="cbEvent">Event CallBack Function</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_RegisterEventCallBackEx_NET(String pEventName, cbEventdelegateEx cbEvent, IntPtr pUser)
        {
            return MV_CC_RegisterEventCallBackEx(handle, pEventName, cbEvent, pUser);
        }
        #endregion

        #region GigEVision ???????
        /// <summary>
        /// Force IP
        /// </summary>
        /// <param name="nIP">IP to set</param>
        /// <param name="nSubNetMask">Subnet mask</param>
        /// <param name="nDefaultGateWay">Default gateway</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_ForceIpEx_NET(UInt32 nIP, UInt32 nSubNetMask, UInt32 nDefaultGateWay)
        {
            return MV_GIGE_ForceIpEx(handle, nIP, nSubNetMask, nDefaultGateWay);
        }

        /// <summary>
        /// IP configuration method
        /// </summary>
        /// <param name="nType">IP type, refer to MV_IP_CFG_x</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetIpConfig_NET(UInt32 nType)
        {
            return MV_GIGE_SetIpConfig(handle, nType);
        }

        /// <summary>
        /// Set to use only one mode,type: MV_NET_TRANS_x. When do not set, priority is to use driver by default
        /// </summary>
        /// <param name="nType">Net transmission mode, refer to MV_NET_TRANS_x</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetNetTransMode_NET(UInt32 nType)
        {
            return MV_GIGE_SetNetTransMode(handle, nType);
        }

        /// <summary>
        /// Get net transmission information
        /// </summary>
        /// <param name="pstInfo">Transmission information</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_GetNetTransInfo_NET(ref MV_NETTRANS_INFO pstInfo)
        {
            return MV_GIGE_GetNetTransInfo(handle, ref pstInfo);
        }

        /// <summary>
        /// Setting the ACK mode of devices Discovery
        /// </summary>
        /// <param name="nMode">ACK mode(Default-Broadcast),0-Unicast,1-Broadcast</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetDiscoveryMode_NET(UInt32 nMode)
        {
            return MV_GIGE_SetDiscoveryMode(nMode);
        }

        /// <summary>
        /// Set GVSP streaming timeout
        /// </summary>
        /// <param name="nMillisec">Timeout, default 300ms, range: >10ms</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetGvspTimeout_NET(UInt32 nMillisec)
        {
            return MV_GIGE_SetGvspTimeout(handle, nMillisec);
        }

        /// <summary>
        /// Get GVSP streaming timeout
        /// </summary>
        /// <param name="pMillisec">Timeout, ms as unit</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_GetGvspTimeout_NET(ref UInt32 pMillisec)
        {
            return MV_GIGE_GetGvspTimeout(handle, ref pMillisec);
        }

        /// <summary>
        /// Set GVCP cammand timeout
        /// </summary>
        /// <param name="nMillisec">Timeout, ms as unit, range: 0-10000</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetGvcpTimeout_NET(UInt32 nMillisec)
        {
            return MV_GIGE_SetGvcpTimeout(handle, nMillisec);
        }

        /// <summary>
        /// Get GVCP cammand timeout
        /// </summary>
        /// <param name="pMillisec">Timeout, ms as unit</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_GetGvcpTimeout_NET(ref UInt32 pMillisec)
        {
            return MV_GIGE_GetGvcpTimeout(handle, ref pMillisec);
        }

        /// <summary>
        /// Set the number of retry GVCP cammand
        /// </summary>
        /// <param name="nRetryGvcpTimes">The number of retries,rang:0-100</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetRetryGvcpTimes_NET(UInt32 nRetryGvcpTimes)
        {
            return MV_GIGE_SetRetryGvcpTimes(handle, nRetryGvcpTimes);
        }

        /// <summary>
        /// Get the number of retry GVCP cammand
        /// </summary>
        /// <param name="pRetryGvcpTimes">The number of retries</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_GetRetryGvcpTimes_NET(ref UInt32 pRetryGvcpTimes)
        {
            return MV_GIGE_GetRetryGvcpTimes(handle, ref pRetryGvcpTimes);
        }

        /// <summary>
        /// Get the optimal Packet Size, Only support GigE Camera
        /// </summary>
        /// <returns>Optimal packet size</returns>
        public Int32 MV_CC_GetOptimalPacketSize_NET()
        {
            return MV_CC_GetOptimalPacketSize(handle);
        }

        /// <summary>
        /// Set whethe to enable resend, and set resend
        /// </summary>
        /// <param name="bEnable">Enable resend</param>
        /// <param name="nMaxResendPercent">Max resend persent</param>
        /// <param name="nResendTimeout">Resend timeout</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetResend_NET(UInt32 bEnable, UInt32 nMaxResendPercent, UInt32 nResendTimeout)
        {
            return MV_GIGE_SetResend(handle, bEnable, nMaxResendPercent, nResendTimeout);
        }

        /// <summary>
        /// Set the max resend retry times
        /// </summary>
        /// <param name="nRetryTimes">The max times to retry resending lost packets,default 20</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetResendMaxRetryTimes_NET(UInt32 nRetryTimes)
        {
            return MV_GIGE_SetResendMaxRetryTimes(handle, nRetryTimes);
        }

        /// <summary>
        /// Get the max resend retry times
        /// </summary>
        /// <param name="pnRetryTimes">the max times to retry resending lost packets</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_GetResendMaxRetryTimes_NET(ref UInt32 pnRetryTimes)
        {
            return MV_GIGE_GetResendMaxRetryTimes(handle, ref pnRetryTimes);
        }

        /// <summary>
        /// Set time interval between same resend requests
        /// </summary>
        /// <param name="nMillisec">The time interval between same resend requests,default 10ms</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetResendTimeInterval_NET(UInt32 nMillisec)
        {
            return MV_GIGE_SetResendTimeInterval(handle, nMillisec);
        }

        /// <summary>
        /// Get time interval between same resend requests
        /// </summary>
        /// <param name="pnMillisec">The time interval between same resend requests</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_GetResendTimeInterval_NET(ref UInt32 pnMillisec)
        {
            return MV_GIGE_GetResendTimeInterval(handle, ref pnMillisec);
        }

        /// <summary>
        /// Set transmission type,Unicast or Multicast
        /// </summary>
        /// <param name="pstTransmissionType">Struct of transmission type</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetTransmissionType_NET(ref MV_CC_TRANSMISSION_TYPE pstTransmissionType)
        {
            return MV_GIGE_SetTransmissionType(handle, ref pstTransmissionType);
        }

        /// <summary>
        /// Issue Action Command
        /// </summary>
        /// <param name="pstActionCmdInfo">Action Command info</param>
        /// <param name="pstActionCmdResults">Action Command Result List</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_GIGE_IssueActionCommand_NET(ref MV_ACTION_CMD_INFO pstActionCmdInfo, ref MV_ACTION_CMD_RESULT_LIST pstActionCmdResults)
        {
            return MV_GIGE_IssueActionCommand(ref pstActionCmdInfo, ref pstActionCmdResults);
        }

        /// <summary>
        /// Get Multicast Status
        /// </summary>
        /// <param name="pstDevInfo">Device Information</param>
        /// <param name="pStatus">Status of Multicast</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public static Int32 MV_GIGE_GetMulticastStatus_NET(ref MV_CC_DEVICE_INFO pstDevInfo, ref Boolean pStatus)
        {
            return MV_GIGE_GetMulticastStatus(ref pstDevInfo, ref pStatus);
        }
        #endregion

        #region CameraLink?????
        /// <summary>
        /// Set device baudrate using one of the CL_BAUDRATE_XXXX value
        /// </summary>
        /// <param name="nBaudrate">Baudrate to set. Refer to the 'CameraParams.h' for parameter definitions, for example, #define MV_CAML_BAUDRATE_9600  0x00000001</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CAML_SetDeviceBaudrate_NET(UInt32 nBaudrate)
        {
            return MV_CAML_SetDeviceBaudrate(handle, nBaudrate);
        }

        public Int32 MV_CAML_SetDeviceBauderate_NET(UInt32 nBaudrate)
        {
            return MV_CAML_SetDeviceBaudrate(handle, nBaudrate);
        }

        /// <summary>
        /// Get device baudrate, using one of the CL_BAUDRATE_XXXX value
        /// </summary>
        /// <param name="pnCurrentBaudrate">Return pointer of baud rate to user. 
        ///                                 Refer to the 'CameraParams.h' for parameter definitions, for example, #define MV_CAML_BAUDRATE_9600  0x00000001</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CAML_GetDeviceBaudrate_NET(ref UInt32 pnCurrentBaudrate)
        {
            return MV_CAML_GetDeviceBaudrate(handle, ref pnCurrentBaudrate);
        }

        public Int32 MV_CAML_GetDeviceBauderate_NET(ref UInt32 pnCurrentBaudrate)
        {
            return MV_CAML_GetDeviceBaudrate(handle, ref pnCurrentBaudrate);
        }

        /// <summary>
        /// Get supported baudrates of the combined device and host interface
        /// </summary>
        /// <param name="pnBaudrateAblity">Return pointer of the supported baudrates to user. 'OR' operation results of the supported baudrates. 
        ///                                Refer to the 'CameraParams.h' for single value definitions, for example, #define MV_CAML_BAUDRATE_9600  0x00000001</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CAML_GetSupportBaudrates_NET(ref UInt32 pnBaudrateAblity)
        {
            return MV_CAML_GetSupportBaudrates(handle, ref pnBaudrateAblity);
        }

        public Int32 MV_CAML_GetSupportBauderates_NET(ref UInt32 pnBaudrateAblity)
        {
            return MV_CAML_GetSupportBaudrates(handle, ref pnBaudrateAblity);
        }

        /// <summary>
        /// Sets the timeout for operations on the serial port
        /// </summary>
        /// <param name="nMillisec">Timeout in [ms] for operations on the serial port.</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CAML_SetGenCPTimeOut_NET(UInt32 nMillisec)
        {
            return MV_CAML_SetGenCPTimeOut(handle, nMillisec);
        }
        #endregion

        #region U3V?????
        /// <summary>
        /// Set transfer size of U3V device
        /// </summary>
        /// <param name="nTransferSize">Transfer size,Byte,default:1M,rang:>=0x10000</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_USB_SetTransferSize_NET(UInt32 nTransferSize)
        {
            return MV_USB_SetTransferSize(handle, nTransferSize);
        }

        /// <summary>
        /// Get transfer size of U3V device
        /// </summary>
        /// <param name="pTransferSize">Transfer size,Byte</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_USB_GetTransferSize_NET(ref UInt32 pTransferSize)
        {
            return MV_USB_GetTransferSize(handle, ref pTransferSize);
        }

        /// <summary>
        /// Set transfer ways of U3V device
        /// </summary>
        /// <param name="nTransferWays">Transfer ways,rang:1-10</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_USB_SetTransferWays_NET(UInt32 nTransferWays)
        {
            return MV_USB_SetTransferWays(handle, nTransferWays);
        }

        /// <summary>
        /// Get transfer ways of U3V device
        /// </summary>
        /// <param name="pTransferWays">Transfer ways</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_USB_GetTransferWays_NET(ref UInt32 pTransferWays)
        {
            return MV_USB_GetTransferWays(handle, ref pTransferWays);
        }
        #endregion

        #region GenTL????,????????(???????)
        /// <summary>
        /// Enumerate interfaces by GenTL
        /// </summary>
        /// <param name="stIFInfoList"> Interface information list</param>
        /// <param name="pGenTLPath">Path of GenTL's cti file</param>
        /// <returns></returns>
        public static Int32 MV_CC_EnumInterfacesByGenTL_NET(ref MV_GENTL_IF_INFO_LIST stIFInfoList, String pGenTLPath)
        {
            return MV_CC_EnumInterfacesByGenTL(ref stIFInfoList, pGenTLPath);
        }

        /// <summary>
        /// Enumerate Device Based On GenTL
        /// </summary>
        /// <param name="stIFInfo">Interface information</param>
        /// <param name="stDevList">Device List</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public static Int32 MV_CC_EnumDevicesByGenTL_NET(ref MV_GENTL_IF_INFO stIFInfo, ref MV_GENTL_DEV_INFO_LIST stDevList)
        {
            return MV_CC_EnumDevicesByGenTL(ref stIFInfo, ref stDevList);
        }

        /// <summary>
        /// Create Device Handle Based On GenTL Device Info
        /// </summary>
        /// <param name="stDevInfo">Device Information Structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_CreateDeviceByGenTL_NET(ref MV_GENTL_DEV_INFO stDevInfo)
        {
            if (IntPtr.Zero != handle)
            {
                MV_CC_DestroyHandle(handle);
                handle = IntPtr.Zero;
            }

            return MV_CC_CreateHandleByGenTL(ref handle, ref stDevInfo);
        }
        #endregion

        #region XML??????
        /// <summary>
        /// Get camera feature tree XML
        /// </summary>
        /// <param name="pData">XML data receiving buffer</param>
        /// <param name="nDataSize">Buffer size</param>
        /// <param name="pnDataLen">Actual data length</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_XML_GetGenICamXML_NET(IntPtr pData, UInt32 nDataSize, ref UInt32 pnDataLen)
        {
            return MV_XML_GetGenICamXML(handle, pData, nDataSize, ref pnDataLen);
        }

        /// <summary>
        /// Get Access mode of cur node
        /// </summary>
        /// <param name="pstrName">Name of node</param>
        /// <param name="pAccessMode">Access mode of the node</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_XML_GetNodeAccessMode_NET(String pstrName, ref MV_XML_AccessMode pAccessMode)
        {
            return MV_XML_GetNodeAccessMode(handle, pstrName, ref pAccessMode);
        }

        /// <summary>
        /// Get Interface Type of cur node
        /// </summary>
        /// <param name="pstrName">Name of node</param>
        /// <param name="pInterfaceType">Interface Type of the node</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_XML_GetNodeInterfaceType_NET(String pstrName, ref MV_XML_InterfaceType pInterfaceType)
        {
            return MV_XML_GetNodeInterfaceType(handle, pstrName, ref pInterfaceType);
        }
        #endregion

        #region ????
        /// <summary>
        /// Save image, support Bmp and Jpeg. Encoding quality(50-99]
        /// </summary>
        /// <param name="stSaveParam">Save image parameters structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_SaveImageEx_NET(ref MV_SAVE_IMAGE_PARAM_EX stSaveParam)
        {
            return MV_CC_SaveImageEx2(handle, ref stSaveParam);
        }

        /// <summary>
        /// Save the image file, support Bmp? Jpeg?Png and Tiff. Encoding quality(50-99]
        /// </summary>
        /// <param name="pstSaveFileParam">Save the image file parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_SaveImageToFile_NET(ref MV_SAVE_IMG_TO_FILE_PARAM pstSaveFileParam)
        {
            return MV_CC_SaveImageToFile(handle, ref pstSaveFileParam);
        }

        /// <summary>
        /// Save 3D point data, support PLY?CSV and OBJ
        /// </summary>
        /// <param name="pstPointDataParam">Save 3D point data parameters structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SavePointCloudData_NET(ref MV_SAVE_POINT_CLOUD_PARAM pstPointDataParam)
        {
            return MV_CC_SavePointCloudData(handle, ref pstPointDataParam);
        }

        /// <summary>
        /// Rotate Image
        /// </summary>
        /// <param name="pstRotateParam">Rotate image parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_RotateImage_NET(ref MV_CC_ROTATE_IMAGE_PARAM pstRotateParam)
        {
            return MV_CC_RotateImage(handle, ref pstRotateParam);
        }

        /// <summary>
        /// Flip Image
        /// </summary>
        /// <param name="pstFlipParam">Flip image parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_FlipImage_NET(ref MV_CC_FLIP_IMAGE_PARAM pstFlipParam)
        {
            return MV_CC_FlipImage(handle, ref pstFlipParam);
        }

        /// <summary>
        /// Pixel format conversion
        /// </summary>
        /// <param name="pstCvtParam">Convert Pixel Type parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_ConvertPixelType_NET(ref MV_PIXEL_CONVERT_PARAM pstCvtParam)
        {
            return MV_CC_ConvertPixelType(handle, ref pstCvtParam);
        }

        /// <summary>
        /// Interpolation algorithm type setting
        /// </summary>
        /// <param name="BayerCvtQuality">Bayer interpolation method  0-Fast 1-Equilibrium 2-Optimal</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_SetBayerCvtQuality_NET(UInt32 BayerCvtQuality)
        {
            return MV_CC_SetBayerCvtQuality(handle, BayerCvtQuality);
        }

        /// <summary>
        /// Set Gamma value
        /// </summary>
        /// <param name="fBayerGammaValue">Gamma value[0.1,4.0]</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_SetBayerGammaValue_NET(Single fBayerGammaValue)
        {
            return MV_CC_SetBayerGammaValue(handle, fBayerGammaValue);
        }

        /// <summary>
        /// Set Gamma param
        /// </summary>
        /// <param name="pstGammaParam">Gamma parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetBayerGammaParam_NET(ref MV_CC_GAMMA_PARAM pstGammaParam)
        {
            return MV_CC_SetBayerGammaParam(handle, ref pstGammaParam);
        }

        /// <summary>
        /// Set CCM param
        /// </summary>
        /// <param name="pstCCMParam">CCM parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetBayerCCMParam_NET(ref MV_CC_CCM_PARAM pstCCMParam)
        {
            return MV_CC_SetBayerCCMParam(handle, ref pstCCMParam);
        }

        /// <summary>
        /// Set CCM param
        /// </summary>
        /// <param name="pstCCMParam">CCM parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetBayerCCMParamEx_NET(ref MV_CC_CCM_PARAM_EX pstCCMParam)
        {
            return MV_CC_SetBayerCCMParamEx(handle, ref pstCCMParam);
        }

        /// <summary>
        /// Set CLUT param
        /// </summary>
        /// <param name="pstCLUTParam">CLUT parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetBayerCLUTParam_NET(ref MV_CC_CLUT_PARAM pstCLUTParam)
        {
            return MV_CC_SetBayerCLUTParam(handle, ref pstCLUTParam);
        }

        /// <summary>
        /// Adjust image contrast
        /// </summary>
        /// <param name="pstContrastParam">Contrast parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_ImageContrast_NET(ref MV_CC_CONTRAST_PARAM pstContrastParam)
        {
            return MV_CC_ImageContrast(handle, ref pstContrastParam);
        }

        /// <summary>
        /// Image sharpen
        /// </summary>
        /// <param name="pstSharpenParam">Sharpen parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_ImageSharpen_NET(ref MV_CC_SHARPEN_PARAM pstSharpenParam)
        {
            return MV_CC_ImageSharpen(handle, ref pstSharpenParam);
        }

        /// <summary>
        /// Color Correct(include CCM and CLUT)
        /// </summary>
        /// <param name="pstColorCorrectParam">Color Correct parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_ColorCorrect_NET(ref MV_CC_COLOR_CORRECT_PARAM pstColorCorrectParam)
        {
            return MV_CC_ColorCorrect(handle, ref pstColorCorrectParam);
        }

        /// <summary>
        /// Noise Estimate
        /// </summary>
        /// <param name="pstNoiseEstimateParam">Noise Estimate parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_NoiseEstimate_NET(ref MV_CC_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam)
        {
            return MV_CC_NoiseEstimate(handle, ref pstNoiseEstimateParam);
        }

        /// <summary>
        /// Spatial Denoise
        /// </summary>
        /// <param name="pstSpatialDenoiseParam">Spatial Denoise parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SpatialDenoise_NET(ref MV_CC_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam)
        {
            return MV_CC_SpatialDenoise(handle, ref pstSpatialDenoiseParam);
        }

        /// <summary>
        /// LSC Calib
        /// </summary>
        /// <param name="pstLSCCalibParam">LSC Calib parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_LSCCalib_NET(ref MV_CC_LSC_CALIB_PARAM pstLSCCalibParam)
        {
            return MV_CC_LSCCalib(handle, ref pstLSCCalibParam);
        }

        /// <summary>
        /// LSC Correct
        /// </summary>
        /// <param name="pstLSCCorrectParam">LSC Correct parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_LSCCorrect_NET(ref MV_CC_LSC_CORRECT_PARAM pstLSCCorrectParam)
        {
            return MV_CC_LSCCorrect(handle, ref pstLSCCorrectParam);
        }

        /// <summary>
        /// High Bandwidth Decode
        /// </summary>
        /// <param name="pstDecodeParam">High Bandwidth Decode parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_HB_Decode_NET(ref MV_CC_HB_DECODE_PARAM pstDecodeParam)
        {
            return MV_CC_HB_Decode(handle, ref pstDecodeParam);
        }

        /// <summary>
        /// Noise estimate of Bayer format
        /// </summary>
        /// <param name="pstNoiseEstimateParam">Noise estimate parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_BayerNoiseEstimate_NET(ref MV_CC_BAYER_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam)
        {
            return MV_CC_BayerNoiseEstimate(handle, ref pstNoiseEstimateParam);
        }

        /// <summary>
        /// Spatial Denoise of Bayer format
        /// </summary>
        /// <param name="pstSpatialDenoiseParam">Spatial Denoise parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_BayerSpatialDenoise_NET(ref MV_CC_BAYER_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam)
        {
            return MV_CC_BayerSpatialDenoise(handle, ref pstSpatialDenoiseParam);
        }

        /// <summary>
        /// Start Grabbing Ex
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_StartGrabbingEx_NET(UInt32 bNeedStart)
        {
            return MV_CC_StartGrabbingEx(handle, bNeedStart);
        }

        /// <summary>
        /// Start Grabbing Ex
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_StopGrabbingEx_NET(UInt32 bNeedStart)
        {
            return MV_CC_StopGrabbingEx(handle, bNeedStart);
        }

        /// <summary>
        /// Save camera feature
        /// </summary>
        /// <param name="pFileName">File name</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_FeatureSave_NET(String pFileName)
        {
            return MV_CC_FeatureSave(handle, pFileName);
        }

        /// <summary>
        /// Load camera feature
        /// </summary>
        /// <param name="pFileName">File name</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_FeatureLoad_NET(String pFileName)
        {
            return MV_CC_FeatureLoad(handle, pFileName);
        }

        /// <summary>
        /// Read the file from the camera
        /// </summary>
        /// <param name="pstFileAccess">File access structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_FileAccessRead_NET(ref MV_CC_FILE_ACCESS pstFileAccess)
        {
            return MV_CC_FileAccessRead(handle, ref pstFileAccess);
        }

        /// <summary>
        /// Write the file to camera
        /// </summary>
        /// <param name="pstFileAccess">File access structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_FileAccessWrite_NET(ref MV_CC_FILE_ACCESS pstFileAccess)
        {
            return MV_CC_FileAccessWrite(handle, ref pstFileAccess);
        }

        /// <summary>
        /// Get File Access Progress 
        /// </summary>
        /// <param name="pstFileAccessProgress">File access Progress</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_GetFileAccessProgress_NET(ref MV_CC_FILE_ACCESS_PROGRESS pstFileAccessProgress)
        {
            return MV_CC_GetFileAccessProgress(handle, ref pstFileAccessProgress);
        }

        /// <summary>
        /// Start Record
        /// </summary>
        /// <param name="pstRecordParam">Record param structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_StartRecord_NET(ref MV_CC_RECORD_PARAM pstRecordParam)
        {
            return MV_CC_StartRecord(handle, ref pstRecordParam);
        }

        /// <summary>
        /// Input RAW data to Record
        /// </summary>
        /// <param name="pstInputFrameInfo">Record data structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_InputOneFrame_NET(ref MV_CC_INPUT_FRAME_INFO pstInputFrameInfo)
        {
            return MV_CC_InputOneFrame(handle, ref pstInputFrameInfo);
        }

        /// <summary>
        /// Stop Record
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_StopRecord_NET()
        {
            return MV_CC_StopRecord(handle);
        }
        #endregion

        #region ???????? Interfaces not recommended
        /// <summary>
        /// Set SDK log path (Interfaces not recommended)
        /// If the logging service MvLogServer is enabled, the interface is invalid and The logging service is enabled by default
        /// </summary>
        /// <param name="pSDKLogPath"></param>
        /// <returns></returns>
        public static Int32 MV_CC_SetSDKLogPath_NET(String pSDKLogPath)
        {
            return MV_CC_SetSDKLogPath(pSDKLogPath);
        }

        /// <summary>
        /// Get basic information of image (Interfaces not recommended)
        /// </summary>
        /// <param name="pstInfo"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetImageInfo_NET(ref MV_IMAGE_BASIC_INFO pstInfo)
        {
            return MV_CC_GetImageInfo(handle, ref pstInfo);
        }

        /// <summary>
        /// Get GenICam proxy (Interfaces not recommended)
        /// </summary>
        /// <returns></returns>
        public IntPtr MV_CC_GetTlProxy_NET()
        {
            return MV_CC_GetTlProxy(handle);
        }

        /// <summary>
        /// Get root node (Interfaces not recommended)
        /// </summary>
        /// <param name="pstNode"></param>
        /// <returns></returns>
        public Int32 MV_XML_GetRootNode_NET(ref MV_XML_NODE_FEATURE pstNode)
        {
            return MV_XML_GetRootNode(handle, ref pstNode);
        }

        /// <summary>
        /// Get all children node of specific node from xml, root node is Root (Interfaces not recommended)
        /// </summary>
        /// <param name="pstNode"></param>
        /// <param name="pstNodesList"></param>
        /// <returns></returns>
        public Int32 MV_XML_GetChildren_NET(ref MV_XML_NODE_FEATURE pstNode, IntPtr pstNodesList)
        {
            return MV_XML_GetChildren(handle, ref pstNode, pstNodesList);
        }

        /// <summary>
        /// Get all children node of specific node from xml, root node is Root (Interfaces not recommended)
        /// </summary>
        /// <param name="pstNode"></param>
        /// <param name="pstNodesList"></param>
        /// <returns></returns>
        public Int32 MV_XML_GetChildren_NET(ref MV_XML_NODE_FEATURE pstNode, ref MV_XML_NODES_LIST pstNodesList)
        {
            return MV_XML_GetChildren(handle, ref pstNode, ref pstNodesList);
        }

        /// <summary>
        /// Get current node feature (Interfaces not recommended)
        /// </summary>
        /// <param name="pstNode"></param>
        /// <param name="pstFeature"></param>
        /// <returns></returns>
        public Int32 MV_XML_GetNodeFeature_NET(ref MV_XML_NODE_FEATURE pstNode, IntPtr pstFeature)
        {
            return MV_XML_GetNodeFeature(handle, ref pstNode, pstFeature);
        }

        /// <summary>
        /// Update node (Interfaces not recommended)
        /// </summary>
        /// <param name="enType"></param>
        /// <param name="pstFeature"></param>
        /// <returns></returns>
        public Int32 MV_XML_UpdateNodeFeature_NET(MV_XML_InterfaceType enType, IntPtr pstFeature)
        {
            return MV_XML_UpdateNodeFeature(handle, enType, pstFeature);
        }

        /// <summary>
        /// Register update callback (Interfaces not recommended)
        /// </summary>
        /// <param name="cbXmlUpdate"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        public Int32 MV_XML_RegisterUpdateCallBack_NET(cbXmlUpdatedelegate cbXmlUpdate, IntPtr pUser)
        {
            return MV_XML_RegisterUpdateCallBack(handle, cbXmlUpdate, pUser);
        }
        #endregion

        #region ?????(???????????)Abandoned interface
        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_GetOneFrameTimeOut
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="nDataSize"></param>
        /// <param name="pFrameInfo"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetOneFrame_NET(IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO pFrameInfo)
        {
            return MV_CC_GetOneFrame(handle, pData, nDataSize, ref pFrameInfo);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_GetOneFrameTimeOut
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="nDataSize"></param>
        /// <param name="pFrameInfo"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetOneFrameEx_NET(IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo)
        {
            return MV_CC_GetOneFrameEx(handle, pData, nDataSize, ref pFrameInfo);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_RegisterImageCallBackEx
        /// </summary>
        /// <param name="cbOutput"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        public Int32 MV_CC_RegisterImageCallBack_NET(cbOutputdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBack(handle, cbOutput, pUser);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_SaveImageEx
        /// </summary>
        /// <param name="stSaveParam"></param>
        /// <returns></returns>
        public Int32 MV_CC_SaveImage_NET(ref MV_SAVE_IMAGE_PARAM stSaveParam)
        {
            return MV_CC_SaveImage(ref stSaveParam);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_GIGE_ForceIpEx
        /// </summary>
        /// <param name="nIP"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_ForceIp_NET(UInt32 nIP)
        {
            return MV_GIGE_ForceIp(handle, nIP);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_RegisterEventCallBackEx
        /// </summary>
        /// <param name="cbEvent"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        public Int32 MV_CC_RegisterEventCallBack_NET(cbEventdelegate cbEvent, IntPtr pUser)
        {
            return MV_CC_RegisterEventCallBack(handle, cbEvent, pUser);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_DisplayOneFrame
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public Int32 MV_CC_Display_NET(IntPtr hWnd)
        {
            return MV_CC_Display(handle, hWnd);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_GetIntValueEx
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetIntValue_NET(String strKey, ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetIntValue(handle, strKey, ref pstValue);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_SetIntValueEx
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetIntValue_NET(String strKey, UInt32 nValue)
        {
            return MV_CC_SetIntValue(handle, strKey, nValue);
        }
        #endregion

        #region ?????????,???????????,?????????????
        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetWidth_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetWidth(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetWidth_NET(UInt32 nValue)
        {
            return MV_CC_SetWidth(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetHeight_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetHeight(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetHeight_NET(UInt32 nValue)
        {
            return MV_CC_SetHeight(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetAOIoffsetX_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAOIoffsetX(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetAOIoffsetX_NET(UInt32 nValue)
        {
            return MV_CC_SetAOIoffsetX(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetAOIoffsetY_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAOIoffsetY(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetAOIoffsetY_NET(UInt32 nValue)
        {
            return MV_CC_SetAOIoffsetY(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetAutoExposureTimeLower_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAutoExposureTimeLower(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetAutoExposureTimeLower_NET(UInt32 nValue)
        {
            return MV_CC_SetAutoExposureTimeLower(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetAutoExposureTimeUpper_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAutoExposureTimeUpper(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetAutoExposureTimeUpper_NET(UInt32 nValue)
        {
            return MV_CC_SetAutoExposureTimeUpper(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetBrightness_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBrightness(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetBrightness_NET(UInt32 nValue)
        {
            return MV_CC_SetBrightness(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetFrameRate_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetFrameRate(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetFrameRate_NET(Single fValue)
        {
            return MV_CC_SetFrameRate(handle, fValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetGain_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetGain(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetGain_NET(Single fValue)
        {
            return MV_CC_SetGain(handle, fValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetExposureTime_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetExposureTime(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetExposureTime_NET(Single fValue)
        {
            return MV_CC_SetExposureTime(handle, fValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetPixelFormat_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetPixelFormat(handle, ref pstValue);
        }

        public Int32 MV_CC_SetPixelFormat_NET(UInt32 nValue)
        {
            return MV_CC_SetPixelFormat(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetAcquisitionMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetAcquisitionMode(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetAcquisitionMode_NET(UInt32 nValue)
        {
            return MV_CC_SetAcquisitionMode(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetGainMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetGainMode(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetGainMode_NET(UInt32 nValue)
        {
            return MV_CC_SetGainMode(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetExposureAutoMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetExposureAutoMode(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetExposureAutoMode_NET(UInt32 nValue)
        {
            return MV_CC_SetExposureAutoMode(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetTriggerMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetTriggerMode(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetTriggerMode_NET(UInt32 nValue)
        {
            return MV_CC_SetTriggerMode(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetTriggerDelay_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetTriggerDelay(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetTriggerDelay_NET(Single fValue)
        {
            return MV_CC_SetTriggerDelay(handle, fValue);
        }

        public Int32 MV_CC_GetTriggerSource_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetTriggerSource(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetTriggerSource_NET(UInt32 nValue)
        {
            return MV_CC_SetTriggerSource(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <returns></returns>
        public Int32 MV_CC_TriggerSoftwareExecute_NET()
        {
            return MV_CC_TriggerSoftwareExecute(handle);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetGammaSelector_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetGammaSelector(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetGammaSelector_NET(UInt32 nValue)
        {
            return MV_CC_SetGammaSelector(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetGamma_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetGamma(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetGamma_NET(Single fValue)
        {
            return MV_CC_SetGamma(handle, fValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetSharpness_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetSharpness(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetSharpness_NET(UInt32 nValue)
        {
            return MV_CC_SetSharpness(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetHue_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetHue(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetHue_NET(UInt32 nValue)
        {
            return MV_CC_SetHue(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetSaturation_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetSaturation(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetSaturation_NET(UInt32 nValue)
        {
            return MV_CC_SetSaturation(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetBalanceWhiteAuto_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetBalanceWhiteAuto(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetBalanceWhiteAuto_NET(UInt32 nValue)
        {
            return MV_CC_SetBalanceWhiteAuto(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetBalanceRatioRed_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBalanceRatioRed(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetBalanceRatioRed_NET(UInt32 nValue)
        {
            return MV_CC_SetBalanceRatioRed(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetBalanceRatioGreen_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBalanceRatioGreen(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetBalanceRatioGreen_NET(UInt32 nValue)
        {
            return MV_CC_SetBalanceRatioGreen(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetBalanceRatioBlue_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBalanceRatioBlue(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetBalanceRatioBlue_NET(UInt32 nValue)
        {
            return MV_CC_SetBalanceRatioBlue(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetDeviceUserID_NET(ref MVCC_STRINGVALUE pstValue)
        {
            return MV_CC_GetDeviceUserID(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="chValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetDeviceUserID_NET(string chValue)
        {
            return MV_CC_SetDeviceUserID(handle, chValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetBurstFrameCount_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBurstFrameCount(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetBurstFrameCount_NET(UInt32 nValue)
        {
            return MV_CC_SetBurstFrameCount(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetAcquisitionLineRate_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAcquisitionLineRate(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetAcquisitionLineRate_NET(UInt32 nValue)
        {
            return MV_CC_SetAcquisitionLineRate(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetHeartBeatTimeout_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetHeartBeatTimeout(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetHeartBeatTimeout_NET(UInt32 nValue)
        {
            return MV_CC_SetHeartBeatTimeout(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_GetGevSCPSPacketSize_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_GIGE_GetGevSCPSPacketSize(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_SetGevSCPSPacketSize_NET(UInt32 nValue)
        {
            return MV_GIGE_SetGevSCPSPacketSize(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_GetGevSCPD_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_GIGE_GetGevSCPD(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_SetGevSCPD_NET(UInt32 nValue)
        {
            return MV_GIGE_SetGevSCPD(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pnIP"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_GetGevSCDA_NET(ref UInt32 pnIP)
        {
            return MV_GIGE_GetGevSCDA(handle, ref pnIP);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nIP"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_SetGevSCDA_NET(UInt32 nIP)
        {
            return MV_GIGE_SetGevSCDA(handle, nIP);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pnPort"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_GetGevSCSP_NET(ref UInt32 pnPort)
        {
            return MV_GIGE_GetGevSCSP(handle, ref pnPort);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nPort"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_SetGevSCSP_NET(UInt32 nPort)
        {
            return MV_GIGE_SetGevSCSP(handle, nPort);
        }
        #endregion

        /// <summary>
        /// Get Camera Handle
        /// </summary>
        /// <returns></returns>
        public IntPtr GetCameraHandle()
        {
            return handle;
        }

        /// <summary>
        /// Byte array to struct
        /// </summary>
        /// <param name="bytes">Byte array</param>
        /// <param name="type">Struct type</param>
        /// <returns>Struct object</returns>
        public static object ByteToStruct(Byte[] bytes, Type type)
        {
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                return null;
            }

            // ?????????
            IntPtr structPtr = Marshal.AllocHGlobal(size);

            // ?byte?????????????
            Marshal.Copy(bytes, 0, structPtr, size);

            // ?????????????
            object obj = Marshal.PtrToStructure(structPtr, type);

            // ??????
            Marshal.FreeHGlobal(structPtr);

            return obj;
        }

        #region ??????
        /// <summary>Unknown Device Type, Reserved</summary>
        public const Int32 MV_UNKNOW_DEVICE = unchecked((Int32)0x00000000);
        /// <summary>GigE Device</summary>
        public const Int32 MV_GIGE_DEVICE = unchecked((Int32)0x00000001);
        /// <summary>1394-a/b Device</summary>
        public const Int32 MV_1394_DEVICE = unchecked((Int32)0x00000002);
        /// <summary>USB3.0 Device</summary>
        public const Int32 MV_USB_DEVICE = unchecked((Int32)0x00000004);
        /// <summary>CameraLink Device</summary>
        public const Int32 MV_CAMERALINK_DEVICE = unchecked((Int32)0x00000008);
        #endregion

        #region ???????
        /// <summary>Successed, no error</summary>
        public const Int32 MV_OK = unchecked((Int32)0x00000000);

        // ???????:??0x80000000-0x800000FF
        /// <summary>Error or invalid handle</summary>
        public const Int32 MV_E_HANDLE = unchecked((Int32)0x80000000);
        /// <summary>Not supported function</summary>
        public const Int32 MV_E_SUPPORT = unchecked((Int32)0x80000001);
        /// <summary>Buffer overflow</summary>
        public const Int32 MV_E_BUFOVER = unchecked((Int32)0x80000002);
        /// <summary>Function calling order error</summary>
        public const Int32 MV_E_CALLORDER = unchecked((Int32)0x80000003);
        /// <summary>Incorrect parameter</summary>
        public const Int32 MV_E_PARAMETER = unchecked((Int32)0x80000004);
        /// <summary>Applying resource failed</summary>
        public const Int32 MV_E_RESOURCE = unchecked((Int32)0x80000006);
        /// <summary>No data</summary>
        public const Int32 MV_E_NODATA = unchecked((Int32)0x80000007);
        /// <summary>Precondition error, or running environment changed</summary>
        public const Int32 MV_E_PRECONDITION = unchecked((Int32)0x80000008);
        /// <summary>Version mismatches</summary>
        public const Int32 MV_E_VERSION = unchecked((Int32)0x80000009);
        /// <summary>Insufficient memory</summary>
        public const Int32 MV_E_NOENOUGH_BUF = unchecked((Int32)0x8000000A);
        /// <summary>Abnormal image, maybe incomplete image because of lost packet</summary>
        public const Int32 MV_E_ABNORMAL_IMAGE = unchecked((Int32)0x8000000B);
        /// <summary>Load library failed</summary>
        public const Int32 MV_E_LOAD_LIBRARY = unchecked((Int32)0x8000000C);
        /// <summary>No Avaliable Buffer</summary>
        public const Int32 MV_E_NOOUTBUF = unchecked((Int32)0x8000000D);
        /// <summary>Encryption error</summary>
        public const Int32 MV_E_ENCRYPT = unchecked((Int32)0x8000000E);
        /// <summary>Unknown error</summary>
        public const Int32 MV_E_UNKNOW = unchecked((Int32)0x800000FF);

        // GenICam????:??0x80000100-0x800001FF
        /// <summary>General error</summary>
        public const Int32 MV_E_GC_GENERIC = unchecked((Int32)0x80000100);
        /// <summary>Illegal parameters</summary>
        public const Int32 MV_E_GC_ARGUMENT = unchecked((Int32)0x80000101);
        /// <summary>The value is out of range</summary>
        public const Int32 MV_E_GC_RANGE = unchecked((Int32)0x80000102);
        /// <summary>Property</summary>
        public const Int32 MV_E_GC_PROPERTY = unchecked((Int32)0x80000103);
        /// <summary>Running environment error</summary>
        public const Int32 MV_E_GC_RUNTIME = unchecked((Int32)0x80000104);
        /// <summary>Logical error</summary>
        public const Int32 MV_E_GC_LOGICAL = unchecked((Int32)0x80000105);
        /// <summary>Node accessing condition error</summary>
        public const Int32 MV_E_GC_ACCESS = unchecked((Int32)0x80000106);
        /// <summary>Timeout</summary>
        public const Int32 MV_E_GC_TIMEOUT = unchecked((Int32)0x80000107);
        /// <summary>Transformation exception</summary>
        public const Int32 MV_E_GC_DYNAMICCAST = unchecked((Int32)0x80000108);
        /// <summary>GenICam unknown error</summary>
        public const Int32 MV_E_GC_UNKNOW = unchecked((Int32)0x800001FF);

        // GigE_STATUS??????:??0x80000200-0x800002FF
        /// <summary>The command is not supported by device</summary>
        public const Int32 MV_E_NOT_IMPLEMENTED = unchecked((Int32)0x80000200);
        /// <summary>The target address being accessed does not exist</summary>
        public const Int32 MV_E_INVALID_ADDRESS = unchecked((Int32)0x80000201);
        /// <summary>The target address is not writable</summary>
        public const Int32 MV_E_WRITE_PROTECT = unchecked((Int32)0x80000202);
        /// <summary>No permission</summary>
        public const Int32 MV_E_ACCESS_DENIED = unchecked((Int32)0x80000203);
        /// <summary>Device is busy, or network disconnected</summary>
        public const Int32 MV_E_BUSY = unchecked((Int32)0x80000204);
        /// <summary>Network data packet error</summary>
        public const Int32 MV_E_PACKET = unchecked((Int32)0x80000205);
        /// <summary>Network error</summary>
        public const Int32 MV_E_NETER = unchecked((Int32)0x80000206);
        /// <summary>Device IP conflict</summary>
        public const Int32 MV_E_IP_CONFLICT = unchecked((Int32)0x80000221);

        // USB_STATUS??????:??0x80000300-0x800003FF
        /// <summary>Reading USB error</summary>
        public const Int32 MV_E_USB_READ = unchecked((Int32)0x80000300);
        /// <summary>Writing USB error</summary>
        public const Int32 MV_E_USB_WRITE = unchecked((Int32)0x80000301);
        /// <summary>Device exception</summary>
        public const Int32 MV_E_USB_DEVICE = unchecked((Int32)0x80000302);
        /// <summary>GenICam error</summary>
        public const Int32 MV_E_USB_GENICAM = unchecked((Int32)0x80000303);
        /// <summary>Insufficient bandwidth, this error code is newly added</summary>
        public const Int32 MV_E_USB_BANDWIDTH = unchecked((Int32)0x80000304);
        /// <summary>Driver mismatch or unmounted drive</summary>
        public const Int32 MV_E_USB_DRIVER = unchecked((Int32)0x80000305);
        /// <summary>USB unknown error</summary>
        public const Int32 MV_E_USB_UNKNOW = unchecked((Int32)0x800003FF);

        // ?????????:??0x80000400-0x800004FF
        /// <summary>Firmware mismatches</summary>
        public const Int32 MV_E_UPG_FILE_MISMATCH = unchecked((Int32)0x80000400);
        /// <summary>Firmware language mismatches</summary>
        public const Int32 MV_E_UPG_LANGUSGE_MISMATCH = unchecked((Int32)0x80000401);
        /// <summary>Upgrading conflicted (repeated upgrading requests during device upgrade)</summary>
        public const Int32 MV_E_UPG_CONFLICT = unchecked((Int32)0x80000402);
        /// <summary>Camera internal error during upgrade</summary>
        public const Int32 MV_E_UPG_INNER_ERR = unchecked((Int32)0x80000403);
        /// <summary>Unknown error during upgrade</summary>
        public const Int32 MV_E_UPG_UNKNOW = unchecked((Int32)0x800004FF);

        #endregion

        #region ??ISP???????
        // ????
        /// <summary>????</summary>
        public const Int32 MV_ALG_OK = unchecked((Int32)0x00000000);
        /// <summary>???????</summary>
        public const Int32 MV_ALG_ERR = unchecked((Int32)0x10000000);

        // ????
        /// <summary>??????????</summary>
        public const Int32 MV_ALG_E_ABILITY_ARG = unchecked((Int32)0x10000001);

        // ????
        /// <summary>??????</summary>
        public const Int32 MV_ALG_E_MEM_NULL = unchecked((Int32)0x10000002);
        /// <summary>?????????</summary>
        public const Int32 MV_ALG_E_MEM_ALIGN = unchecked((Int32)0x10000003);
        /// <summary>????????</summary>
        public const Int32 MV_ALG_E_MEM_LACK = unchecked((Int32)0x10000004);
        /// <summary>?????????????</summary>
        public const Int32 MV_ALG_E_MEM_SIZE_ALIGN = unchecked((Int32)0x10000005);
        /// <summary>???????????</summary>
        public const Int32 MV_ALG_E_MEM_ADDR_ALIGN = unchecked((Int32)0x10000006);

        // ????
        /// <summary>????????????</summary>
        public const Int32 MV_ALG_E_IMG_FORMAT = unchecked((Int32)0x10000007);
        /// <summary>?????????????</summary>
        public const Int32 MV_ALG_E_IMG_SIZE = unchecked((Int32)0x10000008);
        /// <summary>?????step?????</summary>
        public const Int32 MV_ALG_E_IMG_STEP = unchecked((Int32)0x10000009);
        /// <summary>??????????</summary>
        public const Int32 MV_ALG_E_IMG_DATA_NULL = unchecked((Int32)0x1000000A);

        // ????????
        /// <summary>?????????????</summary>
        public const Int32 MV_ALG_E_CFG_TYPE = unchecked((Int32)0x1000000B);
        /// <summary>??????????????????????</summary>
        public const Int32 MV_ALG_E_CFG_SIZE = unchecked((Int32)0x1000000C);
        /// <summary>???????</summary>
        public const Int32 MV_ALG_E_PRC_TYPE = unchecked((Int32)0x1000000D);
        /// <summary>???????????????</summary>
        public const Int32 MV_ALG_E_PRC_SIZE = unchecked((Int32)0x1000000E);
        /// <summary>????????</summary>
        public const Int32 MV_ALG_E_FUNC_TYPE = unchecked((Int32)0x1000000F);
        /// <summary>????????????????</summary>
        public const Int32 MV_ALG_E_FUNC_SIZE = unchecked((Int32)0x10000010);

        // ??????
        /// <summary>index?????</summary>
        public const Int32 MV_ALG_E_PARAM_INDEX = unchecked((Int32)0x10000011);
        /// <summary>value???????????</summary>
        public const Int32 MV_ALG_E_PARAM_VALUE = unchecked((Int32)0x10000012);
        /// <summary>param_num?????</summary>
        public const Int32 MV_ALG_E_PARAM_NUM = unchecked((Int32)0x10000013);

        // ??????
        /// <summary>????????</summary>
        public const Int32 MV_ALG_E_NULL_PTR = unchecked((Int32)0x10000014);
        /// <summary>?????????</summary>
        public const Int32 MV_ALG_E_OVER_MAX_MEM = unchecked((Int32)0x10000015);
        /// <summary>??????</summary>
        public const Int32 MV_ALG_E_CALL_BACK = unchecked((Int32)0x10000016);

        // ?????????
        /// <summary>????</summary>
        public const Int32 MV_ALG_E_ENCRYPT = unchecked((Int32)0x10000017);
        /// <summary>?????????</summary>
        public const Int32 MV_ALG_E_EXPIRE = unchecked((Int32)0x10000018);

        // ?????????????
        /// <summary>???????</summary>
        public const Int32 MV_ALG_E_BAD_ARG = unchecked((Int32)0x10000019);
        /// <summary>???????</summary>
        public const Int32 MV_ALG_E_DATA_SIZE = unchecked((Int32)0x1000001A);
        /// <summary>??step???</summary>
        public const Int32 MV_ALG_E_STEP = unchecked((Int32)0x1000001B);

        // cpu????????
        /// <summary>cpu????????????</summary>
        public const Int32 MV_ALG_E_CPUID = unchecked((Int32)0x1000001C);

        /// <summary>??</summary>
        public const Int32 MV_ALG_WARNING = unchecked((Int32)0x1000001D);

        /// <summary>?????</summary>
        public const Int32 MV_ALG_E_TIME_OUT = unchecked((Int32)0x1000001E);
        /// <summary>???????</summary>
        public const Int32 MV_ALG_E_LIB_VERSION = unchecked((Int32)0x1000001F);
        /// <summary>???????</summary>
        public const Int32 MV_ALG_E_MODEL_VERSION = unchecked((Int32)0x10000020);
        /// <summary>GPU??????</summary>
        public const Int32 MV_ALG_E_GPU_MEM_ALLOC = unchecked((Int32)0x10000021);
        /// <summary>?????</summary>
        public const Int32 MV_ALG_E_FILE_NON_EXIST = unchecked((Int32)0x10000022);
        /// <summary>?????</summary>
        public const Int32 MV_ALG_E_NONE_STRING = unchecked((Int32)0x10000023);
        /// <summary>???????</summary>
        public const Int32 MV_ALG_E_IMAGE_CODEC = unchecked((Int32)0x10000024);
        /// <summary>??????</summary>
        public const Int32 MV_ALG_E_FILE_OPEN = unchecked((Int32)0x10000025);
        /// <summary>??????</summary>
        public const Int32 MV_ALG_E_FILE_READ = unchecked((Int32)0x10000026);
        /// <summary>?????</summary>
        public const Int32 MV_ALG_E_FILE_WRITE = unchecked((Int32)0x10000027);
        /// <summary>????????</summary>
        public const Int32 MV_ALG_E_FILE_READ_SIZE = unchecked((Int32)0x10000028);
        /// <summary>??????</summary>
        public const Int32 MV_ALG_E_FILE_TYPE = unchecked((Int32)0x10000029);
        /// <summary>??????</summary>
        public const Int32 MV_ALG_E_MODEL_TYPE = unchecked((Int32)0x1000002A);
        /// <summary>??????</summary>
        public const Int32 MV_ALG_E_MALLOC_MEM = unchecked((Int32)0x1000002B);
        /// <summary>??????</summary>
        public const Int32 MV_ALG_E_BIND_CORE_FAILED = unchecked((Int32)0x1000002C);

        // ???????
        /// <summary>??????????</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_IMG_FORMAT = unchecked((Int32)0x10402001);
        /// <summary>????????</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_FEATURE_TYPE = unchecked((Int32)0x10402002);
        /// <summary>????????</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_PROFILE_NUM = unchecked((Int32)0x10402003);
        /// <summary>??????????</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_GAIN_NUM = unchecked((Int32)0x10402004);
        /// <summary>???????????</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_GAIN_VAL = unchecked((Int32)0x10402005);
        /// <summary>????????</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_BIN_NUM = unchecked((Int32)0x10402006);
        /// <summary>?????????????</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_INIT_GAIN = unchecked((Int32)0x10402007);
        /// <summary>????????</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_NOT_INIT = unchecked((Int32)0x10402008);
        /// <summary>????????</summary>
        public const Int32 MV_ALG_E_DENOISE_COLOR_MODE = unchecked((Int32)0x10402009);
        /// <summary>??ROI????</summary>
        public const Int32 MV_ALG_E_DENOISE_ROI_NUM = unchecked((Int32)0x1040200a);
        /// <summary>??ROI????</summary>
        public const Int32 MV_ALG_E_DENOISE_ROI_ORI_PT = unchecked((Int32)0x1040200b);
        /// <summary>??ROI????</summary>
        public const Int32 MV_ALG_E_DENOISE_ROI_SIZE = unchecked((Int32)0x1040200c);
        /// <summary>??????????(????????)</summary>
        public const Int32 MV_ALG_E_DENOISE_GAIN_NOT_EXIST = unchecked((Int32)0x1040200d);
        /// <summary>????????????</summary>
        public const Int32 MV_ALG_E_DENOISE_GAIN_BEYOND_RANGE = unchecked((Int32)0x1040200e);
        /// <summary>?????????????</summary>
        public const Int32 MV_ALG_E_DENOISE_NP_BUF_SIZE = unchecked((Int32)0x1040200f);

        #endregion

        #region ?????????
        /// <summary>
        /// ch: GigE???? | en: GigE device information
        /// </summary>
        public struct MV_GIGE_DEVICE_INFO_EX
        {
            public UInt32 nIpCfgOption;
            public UInt32 nIpCfgCurrent;                                        // IP configuration:bit31-static bit30-dhcp bit29-lla
            public UInt32 nCurrentIp;                                           // curtent ip
            public UInt32 nCurrentSubNetMask;                                   // curtent subnet mask
            public UInt32 nDefultGateWay;                                       // current gateway
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chManufacturerName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chModelName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chDeviceVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public String chManufacturerSpecificInfo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public String chSerialNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public Byte[] chUserDefinedName;

            public UInt32 nNetExport;                                           // ??IP??

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        public struct MV_GIGE_DEVICE_INFO
        {
            public UInt32 nIpCfgOption;
            public UInt32 nIpCfgCurrent;                                        // IP configuration:bit31-static bit30-dhcp bit29-lla
            public UInt32 nCurrentIp;                                           // curtent ip
            public UInt32 nCurrentSubNetMask;                                   // curtent subnet mask
            public UInt32 nDefultGateWay;                                       // current gateway
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chManufacturerName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chModelName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chDeviceVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public String chManufacturerSpecificInfo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public String chSerialNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public String chUserDefinedName;

            public UInt32 nNetExport;                                           // ??IP??

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// ch:?????????? | en: Max buffer size of information structs
        /// </summary>
        public const Int32 INFO_MAX_BUFFER_SIZE = 64;

        /// <summary>
        /// ch:USB3 ???? | en:USB3 device information
        /// </summary>
        public struct MV_USB3_DEVICE_INFO_EX
        {
            public Byte CrtlInEndPoint;                                             // ??????
            public Byte CrtlOutEndPoint;                                            // ??????
            public Byte StreamEndPoint;                                             // ???
            public Byte EventEndPoint;                                              // ????
            public UInt16 idVendor;                                                 // ???ID?
            public UInt16 idProduct;                                                // ??ID?
            public UInt32 nDeviceNumber;                                            // ?????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceGUID;                                             // ??GUID?
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;                                             // ?????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;                                              // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chFamilyName;                                             // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;                                          // ?????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chManufacturerName;                                       // ?????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;                                           // ???                                       
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public Byte[] chUserDefinedName;                                        // ???????

            public UInt32 nbcdUSB;                                                  // ???USB??

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public UInt32[] nReserved;                                              // ????
        }

        public struct MV_USB3_DEVICE_INFO
        {
            public Byte CrtlInEndPoint;                                         // ??????
            public Byte CrtlOutEndPoint;                                        // ??????
            public Byte StreamEndPoint;                                         // ???
            public Byte EventEndPoint;                                          // ????
            public UInt16 idVendor;                                             // ???ID?
            public UInt16 idProduct;                                            // ??ID?
            public UInt32 nDeviceNumber;                                        // ?????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceGUID;                                             // ??GUID?
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;                                             // ?????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;                                              // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chFamilyName;                                             // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;                                          // ?????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chManufacturerName;                                       // ?????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;                                           // ???
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chUserDefinedName;                                        // ???????

            public UInt32 nbcdUSB;                                              // ???USB??

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public UInt32[] nReserved;                                          // ????
        }

        /// <summary>
        /// ch:CamLink???? | en:CamLink device information
        /// </summary>
        public struct MV_CamL_DEV_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chPortID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chFamilyName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chManufacturerName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 38)]
            public UInt32[] nReserved;                                          // ????
        }

        /// <summary>
        /// ch:???? | en:Device information
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MV_CC_DEVICE_INFO
        {
            // common info 
            public UInt16 nMajorVer;
            public UInt16 nMinorVer;
            public UInt32 nMacAddrHigh;
            /// MAC ??
            public UInt32 nMacAddrLow;

            public UInt32 nTLayerType;                                          // ?????????,e.g. MV_GIGE_DEVICE

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                          // ????

            /// <summary>
            /// ch:????????? | en:Special devcie information
            /// </summary>
            [StructLayout(LayoutKind.Explicit, Size = 540)]
            public struct SPECIAL_INFO
            {
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 216)]
                public Byte[] stGigEInfo;
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 536)]
                public Byte[] stCamLInfo;
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 540)]
                public Byte[] stUsb3VInfo;
            }
            public SPECIAL_INFO SpecialInfo;
        }

        public const Int32 MV_MAX_DEVICE_NUM = 256;

        public struct MV_CC_DEVICE_INFO_LIST
        {
            public UInt32 nDeviceNum;                                           // ??????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_DEVICE_NUM)]
            public IntPtr[] pDeviceInfo;                                         // ????256???
        }

        /// <summary>
        /// ch:??GenTL????Interface?? | en:Interface Information with GenTL
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MV_GENTL_IF_INFO
        {
            // GenTL??ID
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chInterfaceID;
            // ?????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chTLType;
            // ??????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chDisplayName;
            // GenTL?cti????
            public UInt32 nCtiIndex;
            // ????
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        };

        /// <summary>
        /// ch:??Interface?? | en:Max num of interfaces
        /// </summary>
        public const Int32 MV_MAX_GENTL_IF_NUM = 256;

        /// <summary>
        /// ch:??GenTL?????????? | en:Interface Information List with GenTL
        /// </summary>
        public struct MV_GENTL_IF_INFO_LIST
        {
            //ch:?????? | en:Online Interface Number
            public UInt32 nInterfaceNum;
            //ch:????256??? | en:Support up to 256 Interfaces
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_GENTL_IF_NUM)]
            public IntPtr[] pIFInfo;
        };

        /// <summary>
        /// ch:??GenTL???????? | en:Device Information discovered by with GenTL
        /// </summary>
        public struct MV_GENTL_DEV_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chInterfaceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chTLType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chUserDefinedName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;

            public UInt32 nCtiIndex;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;                                          // ????
        }

        /// <summary>
        /// ch:??GenTL???? | en:Max num of GenTL devices
        /// </summary>
        public const Int32 MV_MAX_GENTL_DEV_NUM = 256;

        /// <summary>
        /// ch:GenTL???? | en:GenTL devices list
        /// </summary>
        public struct MV_GENTL_DEV_INFO_LIST
        {
            public UInt32 nDeviceNum;                                           // ??????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_GENTL_DEV_NUM)]
            public IntPtr[] pDeviceInfo;                            // ????256???
        }

        public struct MV_NETTRANS_INFO
        {
            public Int64 nReviceDataSize;                        // ???????  [??StartGrabbing?StopGrabbing??????]
            public Int32 nThrowFrameCount;                       // ????
            public UInt32 nNetRecvFrameCount;
            public Int64 nRequestResendPacketCount;              // ??????
            public Int64 nResendPacketCount;                     // ????
        }

        public struct MV_FRAME_OUT_INFO
        {
            public UInt16 nWidth;                                     // ???
            public UInt16 nHeight;                                    // ???
            public MvGvspPixelType enPixelType;                       // ????

            public UInt32 nFrameNum;                                  // ??
            public UInt32 nDevTimeStampHigh;                          // ????32?
            public UInt32 nDevTimeStampLow;                           // ????32?
            public UInt32 nReserved0;                                 // ??,8????
            public Int64 nHostTimeStamp;                             // ????????

            public UInt32 nFrameLen;

            public UInt32 nLostPacket;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public UInt32[] nReserved;                                  // ????
        }

        public struct MV_CHUNK_DATA_CONTENT
        {
            public IntPtr pChunkData;
            public UInt32 nChunkID;
            public UInt32 nChunkLen;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;                                  // ????
        }

        public struct MV_FRAME_OUT_INFO_EX
        {
            public UInt16 nWidth;                                     // ???
            public UInt16 nHeight;                                    // ???
            public MvGvspPixelType enPixelType;                       // ????

            public UInt32 nFrameNum;                                  // ??
            public UInt32 nDevTimeStampHigh;                          // ????32?
            public UInt32 nDevTimeStampLow;                           // ????32?
            public UInt32 nReserved0;                                 // ??,8????
            public Int64 nHostTimeStamp;                             // ????????

            public UInt32 nFrameLen;

            // ???chunk??????
            // ??????
            public UInt32 nSecondCount;
            public UInt32 nCycleCount;
            public UInt32 nCycleOffset;

            public Single fGain;
            public Single fExposureTime;
            public UInt32 nAverageBrightness;     //????

            // ?????
            public UInt32 nRed;
            public UInt32 nGreen;
            public UInt32 nBlue;

            public UInt32 nFrameCounter;
            public UInt32 nTriggerIndex;      //????

            //Line ??/??
            public UInt32 nInput;        //??
            public UInt32 nOutput;       //??

            // ROI??
            public UInt16 nOffsetX;
            public UInt16 nOffsetY;

            public UInt16 nChunkWidth;
            public UInt16 nChunkHeight;

            public UInt32 nLostPacket;
            public UInt32 nUnparsedChunkNum;

            [StructLayout(LayoutKind.Explicit)]
            public struct UNPARSED_CHUNK_LIST
            {
                [FieldOffset(0)]
                public IntPtr pUnparsedChunkContent;
                [FieldOffset(0)]
                public Int64 nAligning;
            }
            public UNPARSED_CHUNK_LIST UnparsedChunkList;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
            public UInt32[] nReserved;                                  // ????
        }

        public struct MV_FRAME_OUT
        {
            public IntPtr pBufAddr;

            public MV_FRAME_OUT_INFO_EX stFrameInfo;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] nReserved;                                  // ????
        }

        public enum MV_GRAB_STRATEGY
        {
            MV_GrabStrategy_OneByOne = 0,   // ?????????????(??????)
            MV_GrabStrategy_LatestImagesOnly = 1,   // ????????????(????????????)
            MV_GrabStrategy_LatestImages = 2,   // ??????????,???OutputQueueSize??,???1-ImageNodeNum,???1???LatestImagesOnly
                                                // ,???ImageNodeNum???OneByOne
            MV_GrabStrategy_UpcomingImage = 3,   // ???????
        };

        public struct MV_DISPLAY_FRAME_INFO
        {
            public IntPtr hWnd;

            public IntPtr pData;
            public UInt32 nDataLen;

            public UInt16 nWidth;                                     // ???
            public UInt16 nHeight;                                    // ???
            public MvGvspPixelType enPixelType;                       // ????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                  // ????
        }

        public enum MV_SAVE_IAMGE_TYPE
        {
            MV_Image_Undefined = 0,
            MV_Image_Bmp = 1,
            MV_Image_Jpeg = 2,
            MV_Image_Png = 3,
            MV_Image_Tif = 4,
        };

        public struct MV_SAVE_POINT_CLOUD_PARAM
        {
            public UInt32 nLinePntNum;                 // [IN]     ???????
            public UInt32 nLineNum;                    // [IN]     ??

            public MvGvspPixelType enSrcPixelType;     // [IN]     ?????????
            public IntPtr pSrcData;                    // [IN]     ??????
            public UInt32 nSrcDataLen;                 // [IN]     ??????

            public IntPtr pDstBuf;                     // [OUT]    ????????
            public UInt32 nDstBufSize;                 // [IN]     ??????????(nLinePntNum * nLineNum * (16*3 + 4) + 2048)
            public UInt32 nDstBufLen;                  // [OUT]    ??????????
            public MV_SAVE_POINT_CLOUD_FILE_TYPE enPointCloudFileType;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;              // ????
        };

        public struct MV_SAVE_IMAGE_PARAM
        {
            public IntPtr pData;              // [IN]     ??????
            public UInt32 nDataLen;           // [IN]     ??????
            public MvGvspPixelType enPixelType;        // [IN]     ?????????
            public UInt16 nWidth;             // [IN]     ???
            public UInt16 nHeight;            // [IN]     ???

            public IntPtr pImageBuffer;       // [OUT]    ??????
            public UInt32 nImageLen;          // [OUT]    ??????
            public UInt32 nBufferSize;        // [IN]     ??????????
            public MV_SAVE_IAMGE_TYPE enImageType;        // [IN]     ??????

        };

        public struct MV_SAVE_IMAGE_PARAM_EX
        {
            public IntPtr pData;              // [IN]     ??????
            public UInt32 nDataLen;           // [IN]     ??????
            public MvGvspPixelType enPixelType;        // [IN]     ?????????
            public UInt16 nWidth;             // [IN]     ???
            public UInt16 nHeight;            // [IN]     ???

            public IntPtr pImageBuffer;       // [OUT]    ??????
            public UInt32 nImageLen;          // [OUT]    ??????
            public UInt32 nBufferSize;        // [IN]     ??????????
            public MV_SAVE_IAMGE_TYPE enImageType;        // [IN]     ??????
            public UInt32 nJpgQuality;        // [IN]     ????, (50-99]
            public UInt32 iMethodValue;       // [IN]     Bayer????? 0-?? 1-?? 2-??(?????????????)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public UInt32[] nReserved;                       // ????
        };

        public struct MV_SAVE_IMG_TO_FILE_PARAM
        {
            public MvGvspPixelType enPixelType;        // [IN]     ?????????
            public IntPtr pData;                       // [IN]     ??????
            public UInt32 nDataLen;                    // [IN]     ??????
            public UInt16 nWidth;                      // [IN]     ???
            public UInt16 nHeight;                     // [IN]     ???
            public MV_SAVE_IAMGE_TYPE enImageType;     // [IN]     ??????
            public UInt32 nQuality;                    // [IN]     ????, (0-100]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string pImagePath;                  // [IN]     ??????
            public UInt32 iMethodValue;                // [IN]     Bayer????? 0-?? 1-?? 2-??(?????????????)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;              // ????
        };

        public enum MV_IMG_ROTATION_ANGLE
        {
            MV_IMAGE_ROTATE_90 = 1,
            MV_IMAGE_ROTATE_180 = 2,
            MV_IMAGE_ROTATE_270 = 3,
        };

        public struct MV_CC_ROTATE_IMAGE_PARAM
        {
            public MvGvspPixelType enPixelType;         // [IN]     ????(???Mono8/RGB24/BGR24)
            public UInt32 nWidth;                       // [IN][OUT]     ???
            public UInt32 nHeight;                      // [IN][OUT]     ???

            public IntPtr pSrcData;                     // [IN]     ??????
            public UInt32 nSrcDataLen;                  // [IN]     ??????

            public IntPtr pDstBuf;                      // [OUT]    ??????
            public UInt32 nDstBufLen;                   // [OUT]    ??????
            public UInt32 nDstBufSize;                  // [IN]     ??????????

            public MV_IMG_ROTATION_ANGLE enRotationAngle;   // [IN]     ????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;                       // ????
        };

        public enum MV_IMG_FLIP_TYPE
        {
            MV_FLIP_VERTICAL = 1,
            MV_FLIP_HORIZONTAL = 2,
        };

        public struct MV_CC_FLIP_IMAGE_PARAM
        {
            public MvGvspPixelType enPixelType;         // [IN]     ????(???Mono8/RGB24/BGR24)
            public UInt32 nWidth;                       // [IN]     ???
            public UInt32 nHeight;                      // [IN]     ???

            public IntPtr pSrcData;                     // [IN]     ??????
            public UInt32 nSrcDataLen;                  // [IN]     ??????

            public IntPtr pDstBuf;                      // [OUT]    ??????
            public UInt32 nDstBufLen;                   // [OUT]    ??????
            public UInt32 nDstBufSize;                  // [IN]     ??????????

            public MV_IMG_FLIP_TYPE enFlipType;         // [IN]     ????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;                       // ????
        };

        public struct MV_PIXEL_CONVERT_PARAM
        {
            public UInt16 nWidth;             // [IN]     ???
            public UInt16 nHeight;            // [IN]     ???

            public MvGvspPixelType enSrcPixelType;     // [IN]     ?????
            public IntPtr pSrcData;           // [IN]     ??????
            public UInt32 nSrcDataLen;        // [IN]     ??????

            public MvGvspPixelType enDstPixelType;     // [IN]     ??????
            public IntPtr pDstBuffer;         // [OUT]    ??????
            public UInt32 nDstLen;            // [OUT]    ??????
            public UInt32 nDstBufferSize;     // [IN]     ??????????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nRes;                       // ????
        };

        // Gamma??
        public enum MV_CC_GAMMA_TYPE
        {
            MV_CC_GAMMA_TYPE_NONE = 0,        // ???
            MV_CC_GAMMA_TYPE_VALUE = 1,        // GAMMA?
            MV_CC_GAMMA_TYPE_USER_CURVE = 2,        // GAMMA??,8??????:256*sizeof(unsigned char)
                                                    //            10??????:1024*sizeof(unsigned short)
                                                    //            12??????:4096*sizeof(unsigned short)
                                                    //            16??????:65536*sizeof(unsigned short)
            MV_CC_GAMMA_TYPE_LRGB2SRGB = 3,        // linear RGB to sRGB
            MV_CC_GAMMA_TYPE_SRGB2LRGB = 4,        // sRGB to linear RGB
        };

        public struct MV_CC_GAMMA_PARAM
        {
            public MV_CC_GAMMA_TYPE enGammaType;        // [IN]     Gamma??
            public Single fGammaValue;                  // [IN]     Gamma?
            public IntPtr pGammaCurveBuf;               // [IN]     Gamma????
            public UInt32 nGammaCurveBufLen;            // [IN]     Gamma????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;                       // ????
        };

        public struct MV_CC_CCM_PARAM
        {
            public Boolean bCCMEnable;                  // [IN]     ????CCM
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public Int32[] nCCMat;                      // [IN]     CCM??(-8192~8192)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;                       // ????
        };

        public struct MV_CC_CCM_PARAM_EX
        {
            public Boolean bCCMEnable;                  // [IN]     ????CCM
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public Int32[] nCCMat;                      // [IN]     ??3x3??
            public UInt32 nCCMScale;                    // [IN]     ????(2????)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;                       // ????
        };

        public struct MV_CC_CLUT_PARAM
        {
            public Boolean bCLUTEnable;                 // [IN]     ????CLUT
            public UInt32 nCLUTScale;                   // [IN]     ????(2????)
            public UInt32 nCLUTSize;                    // [IN]     CLUT??,???17
            public IntPtr pCLUTBuf;                     // [OUT]    ??CLUT
            public UInt32 nCLUTBufLen;                  // [IN]     ??CLUT????(nCLUTSize*nCLUTSize*nCLUTSize*sizeof(int)*3)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;                       // ????
        };

        // ???????
        public struct MV_CC_CONTRAST_PARAM
        {
            public UInt32 nWidth;             // [IN]     ????(??8)
            public UInt32 nHeight;            // [IN]     ????(??8)
            public IntPtr pSrcBuf;            // [IN]     ??????
            public UInt32 nSrcBufLen;         // [IN]     ????????
            public MvGvspPixelType enPixelType;    // [IN]     ???????

            public IntPtr pDstBuf;            // [OUT]    ????????
            public UInt32 nDstBufSize;        // [IN]     ??????????
            public UInt32 nDstBufLen;         // [OUT]    ??????????

            public UInt32 nContrastFactor;    // [IN]     ????,??:[1, 10000]

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;                       // ????
        };

        // ????
        public struct MV_CC_SHARPEN_PARAM
        {
            public UInt32 nWidth;             // [IN]     ????(??8)
            public UInt32 nHeight;            // [IN]     ????(??8)
            public IntPtr pSrcBuf;            // [IN]     ??????
            public UInt32 nSrcBufLen;         // [IN]     ????????
            public MvGvspPixelType enPixelType;    // [IN]     ???????

            public IntPtr pDstBuf;            // [OUT]    ????????
            public UInt32 nDstBufSize;        // [IN]     ??????????
            public UInt32 nDstBufLen;         // [OUT]    ??????????

            public UInt32 nSharpenAmount;     // [IN]     ??????,??:[0, 500]
            public UInt32 nSharpenRadius;     // [IN]     ??????(????,????),??:[1, 21]
            public UInt32 nSharpenThreshold;  // [IN]     ??????,??:[0, 255]

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;                       // ????
        };

        // ??????(??CCM?CLUT)
        public struct MV_CC_COLOR_CORRECT_PARAM
        {
            public UInt32 nWidth;             // [IN]     ????
            public UInt32 nHeight;            // [IN]     ????
            public IntPtr pSrcBuf;            // [IN]     ??????
            public UInt32 nSrcBufLen;         // [IN]     ????????
            public MvGvspPixelType enPixelType;    // [IN]     ???????

            public IntPtr pDstBuf;            // [OUT]    ????????
            public UInt32 nDstBufSize;        // [IN]     ??????????
            public UInt32 nDstBufLen;         // [OUT]    ??????????

            public UInt32 nImageBit;          // [IN]     ????????,8 or 10 or 12 or 16
            public MV_CC_GAMMA_PARAM stGammaParam;       // [IN]     ??Gamma??
            public MV_CC_CCM_PARAM_EX stCCMParam;         // [IN]     ??CCM??
            public MV_CC_CLUT_PARAM stCLUTParam;        // [IN]     ??CLUT??

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;                       // ????
        };

        // ??ROI??
        public struct MV_CC_RECT_I
        {
            public UInt32 nX;                        // [IN]     ?????X???
            public UInt32 nY;                        // [IN]     ?????Y???
            public UInt32 nWidth;                    // [IN]     ????
            public UInt32 nHeight;                   // [IN]     ????
        };

        // ??????
        public struct MV_CC_NOISE_ESTIMATE_PARAM
        {
            public UInt32 nWidth;             // [IN]     ????
            public UInt32 nHeight;            // [IN]     ????
            public MvGvspPixelType enPixelType;    // [IN]     ???????
            public IntPtr pSrcBuf;            // [IN]     ??????
            public UInt32 nSrcBufLen;         // [IN]     ????????

            public IntPtr pstROIRect;         // [IN]     ??ROI
            public UInt32 nROINum;            // [IN]     ROI??

            //Bayer???????,Mono8/RGB???
            public UInt32 nNoiseThreshold;    // [IN]     ????[0-4095]

            public IntPtr pNoiseProfile;      // [OUT]    ??????
            public UInt32 nNoiseProfileSize;  // [IN]     ??????????
            public UInt32 nNoiseProfileLen;   // [OUT]    ????????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;                       // ????
        };

        // ??????
        public struct MV_CC_SPATIAL_DENOISE_PARAM
        {
            public UInt32 nWidth;             // [IN]     ????
            public UInt32 nHeight;            // [IN]     ????
            public MvGvspPixelType enPixelType;    // [IN]     ???????
            public IntPtr pSrcBuf;            // [IN]     ??????
            public UInt32 nSrcBufLen;         // [IN]     ????????

            public IntPtr pDstBuf;            // [OUT]    ????????
            public UInt32 nDstBufSize;        // [IN]     ??????????
            public UInt32 nDstBufLen;         // [OUT]    ??????????

            public IntPtr pNoiseProfile;      // [IN]     ??????
            public UInt32 nNoiseProfileLen;   // [IN]     ????????

            //Bayer?????????,Mono8/RGB???
            public UInt32 nBayerDenoiseStrength;// [IN]     ????(0-100)
            public UInt32 nBayerSharpenStrength;// [IN]     ????(0-32)
            public UInt32 nBayerNoiseCorrect; // [IN]     ??????(0-1280)

            //Mono8/RGB?????????,Bayer???
            public UInt32 nNoiseCorrectLum;   // [IN]     ??????(1-2000)
            public UInt32 nNoiseCorrectChrom; // [IN]     ??????(1-2000)
            public UInt32 nStrengthLum;       // [IN]     ??????(0-100)
            public UInt32 nStrengthChrom;     // [IN]     ??????(0-100)
            public UInt32 nStrengthSharpen;   // [IN]     ????(1-1000)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;                       // ????
        };

        // LSC????
        public struct MV_CC_LSC_CALIB_PARAM
        {
            public UInt32 nWidth;             // [IN]     ????(16~65536)
            public UInt32 nHeight;            // [IN]     ????(16~65536)
            public MvGvspPixelType enPixelType;    // [IN]     ???????
            public IntPtr pSrcBuf;            // [IN]     ??????
            public UInt32 nSrcBufLen;         // [IN]     ????????

            public IntPtr pCalibBuf;          // [OUT]    ???????
            public UInt32 nCalibBufSize;      // [IN]     ??????????(nWidth*nHeight*sizeof(unsigned short))
            public UInt32 nCalibBufLen;       // [OUT]    ?????????

            public UInt32 nSecNumW;           // [IN]     ?????
            public UInt32 nSecNumH;           // [IN]     ?????
            public UInt32 nPadCoef;           // [IN]     ??????,??1~5
            public UInt32 nCalibMethod;       // [IN]     ????,0-?????
                                              //                    1-???????
                                              //                    2-????

            public UInt32 nTargetGray;        // [IN]     ????(8bits,[0,255])
                                              //                  (10bits,[0,1023])
                                              //                  (12bits,[0,4095])
                                              //                  (16bits,[0,65535])

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;                       // ????
        };

        // LSC????
        public struct MV_CC_LSC_CORRECT_PARAM
        {
            public UInt32 nWidth;             // [IN]     ????(16~65536)
            public UInt32 nHeight;            // [IN]     ????(16~65536)
            public MvGvspPixelType enPixelType;    // [IN]     ???????
            public IntPtr pSrcBuf;            // [IN]     ??????
            public UInt32 nSrcBufLen;         // [IN]     ????????

            public IntPtr pDstBuf;            // [OUT]    ????????
            public UInt32 nDstBufSize;        // [IN]     ??????????
            public UInt32 nDstBufLen;         // [OUT]    ??????????

            public IntPtr pCalibBuf;          // [IN]     ???????
            public UInt32 nCalibBufLen;       // [IN]     ?????????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;                       // ????
        };

        // ??????
        public enum MV_CC_BAYER_NOISE_FEATURE_TYPE
        {
            MV_CC_BAYER_NOISE_FEATURE_TYPE_INVALID = 0, // ?? 
            MV_CC_BAYER_NOISE_FEATURE_TYPE_PROFILE = 1, // ????
            MV_CC_BAYER_NOISE_FEATURE_TYPE_LEVEL = 2, // ????
            MV_CC_BAYER_NOISE_FEATURE_TYPE_DEFAULT = 2, // ???
        };

        public struct MV_CC_BAYER_NOISE_PROFILE_INFO
        {
            public UInt32 nVersion;           // ??
            public MV_CC_BAYER_NOISE_FEATURE_TYPE enNoiseFeatureType;  // ??????
            public MvGvspPixelType enPixelType;    // ????
            public Int32 nNoiseLevel;        // ??????
            public UInt32 nCurvePointNum;     // ????
            public IntPtr nNoiseCurve;        // ????
            public IntPtr nLumCurve;          // ????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;               // ????
        };

        public struct MV_CC_BAYER_NOISE_ESTIMATE_PARAM
        {
            public UInt32 nWidth;               // [IN]     ???(????8)
            public UInt32 nHeight;              // [IN]     ???(????8)
            public MvGvspPixelType enPixelType; // [IN]     ????

            public IntPtr pSrcData;             // [IN]     ??????
            public UInt32 nSrcDataLen;          // [IN]     ??????

            public UInt32 nNoiseThreshold;      // [IN]     ????(0-4095)

            public IntPtr pCurveBuf;            // [IN]     ?????????????(??????,????:4096 * sizeof(int) * 2)
            public MV_CC_BAYER_NOISE_PROFILE_INFO stNoiseProfile;   // [OUT]    ??????

            public UInt32 nThreadNum;           // [IN]     ????,0????????????;1?????(??);??1??????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;               // ????
        };

        public struct MV_CC_BAYER_SPATIAL_DENOISE_PARAM
        {
            public UInt32 nWidth;               // [IN]     ???(????8)
            public UInt32 nHeight;              // [IN]     ???(????8)
            public MvGvspPixelType enPixelType; // [IN]     ????

            public IntPtr pSrcData;             // [IN]     ??????
            public UInt32 nSrcDataLen;          // [IN]     ??????

            public IntPtr pDstBuf;              // [OUT]    ????????
            public UInt32 nDstBufSize;          // [IN]     ??????????
            public UInt32 nDstBufLen;           // [OUT]    ??????????

            public MV_CC_BAYER_NOISE_PROFILE_INFO stNoiseProfile;   // [IN]    ??????(???????)
            public UInt32 nDenoiseStrength;     // [IN]     ????(0-100) 
            public UInt32 nSharpenStrength;     // [IN]     ????(0-32)
            public UInt32 nNoiseCorrect;        // [IN]     ??????(0-1280)

            public UInt32 nThreadNum;           // [IN]     ????,0????????????;1?????(??);??1??????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;               // ????
        };

        public struct MV_CC_FRAME_SPEC_INFO
        {
            //??????
            public UInt32 nSecondCount;         // [OUT]     ??
            public UInt32 nCycleCount;          // [OUT]     ???
            public UInt32 nCycleOffset;         // [OUT]     ?????

            public Single fGain;                // [OUT]     ??
            public Single fExposureTime;        // [OUT]     ????
            public UInt32 nAverageBrightness;   // [OUT]     ????

            //?????
            public UInt32 nRed;                 // [OUT]     ??
            public UInt32 nGreen;               // [OUT]     ??
            public UInt32 nBlue;                // [OUT]     ??

            public UInt32 nFrameCounter;        // [OUT]     ???
            public UInt32 nTriggerIndex;        // [OUT]     ????

            public UInt32 nInput;               // [OUT]     ??
            public UInt32 nOutput;              // [OUT]     ??

            public UInt16 nOffsetX;             // [OUT]     ?????
            public UInt16 nOffsetY;             // [OUT]     ?????
            public UInt16 nFrameWidth;          // [OUT]     ???
            public UInt16 nFrameHeight;         // [OUT]     ???

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] nRes;               // ????
        };

        public struct MV_CC_HB_DECODE_PARAM
        {
            public IntPtr pSrcBuf;              // [IN]     ??????
            public UInt32 nSrcLen;              // [IN]     ??????

            public UInt32 nWidth;               // [OUT]    ???
            public UInt32 nHeight;              // [OUT]    ???
            public IntPtr pDstBuf;              // [OUT]    ??????
            public UInt32 nDstBufSize;          // [IN]     ??????????
            public UInt32 nDstBufLen;           // [OUT]    ??????
            public MvGvspPixelType enDstPixelType;  // [OUT]     ???????

            public MV_CC_FRAME_SPEC_INFO stFrameSpecInfo;   // [OUT]    ????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;               // ????
        };

        // ??????
        public enum MV_RECORD_FORMAT_TYPE
        {
            MV_FormatType_Undefined = 0,
            MV_FormatType_AVI = 1,
        };

        // ch:??3D???? | en:Save 3D file
        public enum MV_SAVE_POINT_CLOUD_FILE_TYPE
        {
            MV_PointCloudFile_Undefined = 0,
            MV_PointCloudFile_PLY = 1,
            MV_PointCloudFile_CSV = 2,
            MV_PointCloudFile_OBJ = 3,
        };

        public struct MV_CC_RECORD_PARAM
        {
            public MvGvspPixelType enPixelType;// [IN]     ?????????

            public UInt16 nWidth;              // [IN]     ???(?????????8???)
            public UInt16 nHeight;             // [IN]     ???(?????????8???)

            public Single fFrameRate;          // [IN]     ??fps(??1/16)
            public UInt32 nBitRate;            // [IN]     ??kbps(128kbps-16Mbps)

            public MV_RECORD_FORMAT_TYPE enRecordFmtType;// [IN]     ????

            public String strFilePath;         // [IN]     ????????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;              // ????
        };

        public struct MV_CC_INPUT_FRAME_INFO
        {
            public IntPtr pData;              // [IN]     ??????
            public UInt32 nDataLen;           // [IN]     ????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;              // ????
        };

        // ????
        public enum MV_CAM_ACQUISITION_MODE
        {
            MV_ACQ_MODE_SINGLE = 0,            // ????
            MV_ACQ_MODE_MUTLI = 1,            // ????
            MV_ACQ_MODE_CONTINUOUS = 2,            // ??????
        };

        // ????
        public enum MV_CAM_GAIN_MODE
        {
            MV_GAIN_MODE_OFF = 0,            // ??
            MV_GAIN_MODE_ONCE = 1,            // ??
            MV_GAIN_MODE_CONTINUOUS = 2,            // ??
        };

        // ????
        public enum MV_CAM_EXPOSURE_MODE
        {
            MV_EXPOSURE_MODE_TIMED = 0,            // Timed
            MV_EXPOSURE_MODE_TRIGGER_WIDTH = 1,            // TriggerWidth
        };

        // ??????
        public enum MV_CAM_EXPOSURE_AUTO_MODE
        {
            MV_EXPOSURE_AUTO_MODE_OFF = 0,            // ??
            MV_EXPOSURE_AUTO_MODE_ONCE = 1,            // ??
            MV_EXPOSURE_AUTO_MODE_CONTINUOUS = 2,            // ??
        };

        public enum MV_CAM_TRIGGER_MODE
        {
            MV_TRIGGER_MODE_OFF = 0,            // ??
            MV_TRIGGER_MODE_ON = 1,            // ??
        };

        public enum MV_CAM_GAMMA_SELECTOR
        {
            MV_GAMMA_SELECTOR_USER = 1,
            MV_GAMMA_SELECTOR_SRGB = 2,
        };

        public enum MV_CAM_BALANCEWHITE_AUTO
        {
            MV_BALANCEWHITE_AUTO_OFF = 0,
            MV_BALANCEWHITE_AUTO_ONCE = 2,
            MV_BALANCEWHITE_AUTO_CONTINUOUS = 1,            // ??
        }

        public enum MV_CAM_TRIGGER_SOURCE
        {
            MV_TRIGGER_SOURCE_LINE0 = 0,
            MV_TRIGGER_SOURCE_LINE1 = 1,
            MV_TRIGGER_SOURCE_LINE2 = 2,
            MV_TRIGGER_SOURCE_LINE3 = 3,
            MV_TRIGGER_SOURCE_COUNTER0 = 4,

            MV_TRIGGER_SOURCE_SOFTWARE = 7,
            MV_TRIGGER_SOURCE_FrequencyConverter = 8,
        };

        public enum MV_GIGE_TRANSMISSION_TYPE
        {
            MV_GIGE_TRANSTYPE_UNICAST = 0x0,                // ch:????(??) | en:Unicast mode
            MV_GIGE_TRANSTYPE_MULTICAST = 0x1,                // ch:???? | en:Multicast mode
            MV_GIGE_TRANSTYPE_LIMITEDBROADCAST = 0x2,                // ch:????????,???? | en:Limited broadcast mode,not support
            MV_GIGE_TRANSTYPE_SUBNETBROADCAST = 0x3,                // ch:???????,???? | en:Subnet broadcast mode,not support
            MV_GIGE_TRANSTYPE_CAMERADEFINED = 0x4,                // ch:???????,???? | en:Transtype from camera,not support
            MV_GIGE_TRANSTYPE_UNICAST_DEFINED_PORT = 0x5,                // ch:????????????????Port? | en:User Defined Receive Data Port
            MV_GIGE_TRANSTYPE_UNICAST_WITHOUT_RECV = 0x00010000,         // ch:???????,??????????? | en:Unicast without receive data
            MV_GIGE_TRANSTYPE_MULTICAST_WITHOUT_RECV = 0x00010001,         // ch:??????,??????????? | en:Multicast without receive data
        };

        // GigEVision IP Configuration
        public const Int32 MV_IP_CFG_STATIC = 0x05000000;
        public const Int32 MV_IP_CFG_DHCP = 0x06000000;
        public const Int32 MV_IP_CFG_LLA = 0x04000000;

        // GigEVision Net Transfer Mode
        public const Int32 MV_NET_TRANS_DRIVER = 0x00000001;
        public const Int32 MV_NET_TRANS_SOCKET = 0x00000002;

        // CameraLink Baud Rates (CLUINT32)
        public const Int32 MV_CAML_BAUDRATE_9600 = 0x00000001;
        public const Int32 MV_CAML_BAUDRATE_19200 = 0x00000002;
        public const Int32 MV_CAML_BAUDRATE_38400 = 0x00000004;
        public const Int32 MV_CAML_BAUDRATE_57600 = 0x00000008;
        public const Int32 MV_CAML_BAUDRATE_115200 = 0x00000010;
        public const Int32 MV_CAML_BAUDRATE_230400 = 0x00000020;
        public const Int32 MV_CAML_BAUDRATE_460800 = 0x00000040;
        public const Int32 MV_CAML_BAUDRATE_921600 = 0x00000080;
        public const Int32 MV_CAML_BAUDRATE_AUTOMAX = 0x40000000;

        // ????
        public const Int32 MV_MATCH_TYPE_NET_DETECT = 0x00000001;      // ?????????
        public const Int32 MV_MATCH_TYPE_USB_DETECT = 0x00000002;      // host?????U3V???????

        public const Int32 MV_MAX_XML_DISC_STRLEN_C = 512;
        public const Int32 MV_MAX_XML_NODE_STRLEN_C = 64;
        public const Int32 MV_MAX_XML_NODE_NUM_C = 128;
        public const Int32 MV_MAX_XML_SYMBOLIC_NUM = 64;
        public const Int32 MV_MAX_XML_STRVALUE_STRLEN_C = 64;
        public const Int32 MV_MAX_XML_PARENTS_NUM = 8;
        public const Int32 MV_MAX_XML_SYMBOLIC_STRLEN_C = 64;

        public struct MV_ALL_MATCH_INFO
        {
            public UInt32 nType;                                  // ?????????,e.g. MV_MATCH_TYPE_NET_DETECT
            public IntPtr pInfo;                                  // ???????,??????
            public UInt32 nInfoSize;                              // ???????
        }

        public struct MV_MATCH_INFO_NET_DETECT
        {
            public Int64 nReviceDataSize;    // ???????  [??StartGrabbing?StopGrabbing??????]
            public Int64 nLostPacketCount;   // ??????
            public UInt32 nLostFrameCount;    // ????
            public UInt32 nNetRecvFrameCount;
            public Int64 nRequestResendPacketCount;// ??????
            public Int64 nResendPacketCount;  // ????
        }

        public struct MV_MATCH_INFO_USB_DETECT
        {
            public Int64 nReviceDataSize;      // ???????    [??OpenDevicce?CloseDevice??????]
            public UInt32 nRevicedFrameCount;   // ??????
            public UInt32 nErrorFrameCount;     // ????
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public UInt32[] nReserved;                       // ????
        }

        public struct MV_IMAGE_BASIC_INFO
        {
            // width
            public UInt16 nWidthValue;
            public UInt16 nWidthMin;
            public UInt32 nWidthMax;
            public UInt32 nWidthInc;

            // height
            public UInt32 nHeightValue;
            public UInt32 nHeightMin;
            public UInt32 nHeightMax;
            public UInt32 nHeightInc;

            // framerate
            public Single fFrameRateValue;
            public Single fFrameRateMin;
            public Single fFrameRateMax;

            // ????
            public UInt32 enPixelType;                            // ???????
            public UInt32 nSupportedPixelFmtNum;                  // ?????????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_XML_SYMBOLIC_NUM)]
            public UInt32[] enPixelList;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;                                          // ????
        }

        //  ??????
        public const Int32 MV_EXCEPTION_DEV_DISCONNECT = 0x00008001;      // ??????
        public const Int32 MV_EXCEPTION_VERSION_CHECK = 0x00008002;      // SDK????????

        // ???????

        public const Int32 MV_ACCESS_Exclusive = 1;// ????,??APP????CCP???
        public const Int32 MV_ACCESS_ExclusiveWithSwitch = 2;// ???5???????,?????????
        public const Int32 MV_ACCESS_Control = 3;// ????,??APP????????
        public const Int32 MV_ACCESS_ControlWithSwitch = 4;// ???5????????,?????????
        public const Int32 MV_ACCESS_ControlSwitchEnable = 5;// ????????????
        public const Int32 MV_ACCESS_ControlSwitchEnableWithKey = 6;// ???5????????,??????????????
        public const Int32 MV_ACCESS_Monitor = 7;// ???????,????????

        // ???????????
        public enum MV_XML_InterfaceType
        {
            IFT_IValue,                                                         // IValue interface
            IFT_IBase,                                                          // IBase interface
            IFT_IInteger,                                                       // IInteger interface
            IFT_IBoolean,                                                       // IBoolean interface
            IFT_ICommand,                                                       // ICommand interface
            IFT_IFloat,                                                         // IFloat interface
            IFT_IString,                                                        // IString interface
            IFT_IRegister,                                                      // IRegister interface
            IFT_ICategory,                                                      // ICategory interface
            IFT_IEnumeration,                                                   // IEnumeration interface
            IFT_IEnumEntry,                                                     // IEnumEntry interface
            IFT_IPort                                                           // IPort interface
        };

        public enum MV_XML_AccessMode
        {
            AM_NI,                                                              // Not implemented
            AM_NA,                                                              // Not available
            AM_WO,                                                              // Write Only
            AM_RO,                                                              // Read Only
            AM_RW,                                                              // Read and Write
            AM_Undefined,                                                       // Object is not yet initialized
            AM_CycleDetect                                                      // used internally for AccessMode cycle detection
        };

        public enum MV_XML_Visibility
        {
            V_Beginner = 0,                                                     // Always visible
            V_Expert = 1,                                                     // Visible for experts or Gurus
            V_Guru = 2,                                                     // Visible for Gurus
            V_Invisible = 3,                                                     // Not Visible
            V_Undefined = 99                                                     // Object is not yet initialized
        };

        //Event??????
        public const Int32 MAX_EVENT_NAME_SIZE = 128;//??Event????????

        public struct MV_EVENT_OUT_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_EVENT_NAME_SIZE)]
            public string EventName;

            public UInt16 nEventID;                           //Event?
            public UInt16 nStreamChannel;                     //?????

            public UInt32 nBlockIdHigh;                       //????
            public UInt32 nBlockIdLow;                        //????

            public UInt32 nTimestampHigh;                     //?????
            public UInt32 nTimestampLow;                      //?????

            public IntPtr pEventData;                         //Event??
            public UInt32 nEventDataSize;                     //Event????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] nReserved;                                          // ????
        };

        // ????
        public struct MV_CC_FILE_ACCESS
        {
            public String pUserFileName;                         //?????
            public String pDevFileName;                          //?????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public UInt32[] nReserved;                                          // ????
        }

        // ??????
        public struct MV_CC_FILE_ACCESS_PROGRESS
        {
            public Int64 nCompleted;                         //??????
            public Int64 nTotal;                             //???

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;                                          // ????
        }

        // ????,?????????????
        public struct MV_CC_TRANSMISSION_TYPE
        {
            public MV_GIGE_TRANSMISSION_TYPE enTransmissionType; //????
            public UInt32 nDestIp;                                 //??IP,????????
            public UInt16 nDestPort;                             //??Port,????????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public UInt32[] nReserved;                                          // ????
        }

        // ??????
        public struct MV_ACTION_CMD_INFO
        {
            public UInt32 nDeviceKey;        //????
            public UInt32 nGroupKey;         //??
            public UInt32 nGroupMask;        //???

            public UInt32 bActionTimeEnable; //?????1?Action Time???,?1???
            public Int64 nActionTime;       //?????,?????

            public String pBroadcastAddress; //?????
            public UInt32 nTimeOut;          //??ACK?????,???0?????ACK

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MV_ACTION_CMD_RESULT
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public String strDeviceAddress;       //IP address of the device

            public Int32 nStatus;                 //status code returned by the device

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MV_ACTION_CMD_RESULT_LIST
        {
            public UInt32 nNumResults;                 //?????

            public IntPtr pResults;
        }

        public struct MV_XML_NODE_FEATURE
        {
            public MV_XML_InterfaceType enType;                                 // ????
            public MV_XML_Visibility enVisivility;                           // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;                                              // ???
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;                                           // ??

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MV_XML_NODES_LIST
        {
            public UInt32 nNodeNum;                               // ????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_XML_NODE_NUM_C)]
            public MV_XML_NODE_FEATURE[] stNodes;
        }

        public struct MVCC_INTVALUE
        {
            public UInt32 nCurValue;                              // ???
            public UInt32 nMax;
            public UInt32 nMin;
            public UInt32 nInc;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MVCC_INTVALUE_EX
        {
            public Int64 nCurValue;                              // ???
            public Int64 nMax;
            public Int64 nMin;
            public Int64 nInc;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MVCC_FLOATVALUE
        {
            public Single fCurValue;                              // ???
            public Single fMax;
            public Single fMin;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MVCC_ENUMVALUE
        {
            public UInt32 nCurValue;                              // ???
            public UInt32 nSupportedNum;                          // ??????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_XML_SYMBOLIC_NUM)]
            public UInt32[] nSupportValue;                                      // ????

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MVCC_STRINGVALUE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string chCurValue;                                           // ???

            public Int64 nMaxLength;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MV_XML_FEATURE_Integer
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;                                              // ???
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;                                           // ??

            public MV_XML_Visibility enVisivility;                           // ????
            public MV_XML_AccessMode enAccessMode;                           // ????
            public Int32 bIsLocked;                              // ?????0-?;1-?
            public Int64 nValue;                                 // ???
            public Int64 nMinValue;                              // ???
            public Int64 nMaxValue;                              // ???
            public Int64 nIncrement;                             // ??

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MV_XML_FEATURE_Boolean
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;                                              // ???
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;                                           // ??

            public MV_XML_Visibility enVisivility;                              // ????
            public MV_XML_AccessMode enAccessMode;                              // ????
            public Int32 bIsLocked;                                             // ?????0-?;1-?
            public bool bValue;                                                 // ???

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MV_XML_FEATURE_Command
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;                                              // ???
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;                                           // ??

            public MV_XML_Visibility enVisivility;                              // ????
            public MV_XML_AccessMode enAccessMode;                              // ????
            public Int32 bIsLocked;                                             // ?????0-?;1-?

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MV_XML_FEATURE_Float
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;                                              // ???
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;                                           // ??

            public MV_XML_Visibility enVisivility;                              // ????
            public MV_XML_AccessMode enAccessMode;                              // ????
            public Int32 bIsLocked;                                             // ?????0-?;1-?
            public Double dfValue;                                              // ???
            public Double dfMinValue;                                           // ???
            public Double dfMaxValue;                                           // ???
            public Double dfIncrement;                                          // ??

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MV_XML_FEATURE_String
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;                                              // ???
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;                                           // ??

            public MV_XML_Visibility enVisivility;                              // ????
            public MV_XML_AccessMode enAccessMode;                              // ????
            public Int32 bIsLocked;                                             // ?????0-?;1-?
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_STRVALUE_STRLEN_C)]
            public string strValue;                                              // ???

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MV_XML_FEATURE_Register
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;                                              // ???
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;                                           // ??

            public MV_XML_Visibility enVisivility;                              // ????
            public MV_XML_AccessMode enAccessMode;                              // ????
            public Int32 bIsLocked;                                             // ?????0-?;1-?
            public Int64 nAddrValue;                                            // ???

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MV_XML_FEATURE_Category
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;                                              // ???
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;                                           // ??
            public MV_XML_Visibility enVisivility;                              // ????
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MV_XML_FEATURE_EnumEntry
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;                                              // ???
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;                                           // ??

            public Int32 bIsImplemented;
            public Int32 nParentsNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_XML_PARENTS_NUM)]
            public MV_XML_NODE_FEATURE[] stParentsList;

            public MV_XML_Visibility enVisivility;                           //????
            public Int64 nValue;                                 // ???
            public MV_XML_AccessMode enAccessMode;                           // ????
            public Int32 bIsLocked;                              // ?????0-?;1-?

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;                                          // ????
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct StrSymbolic
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_SYMBOLIC_STRLEN_C)]
            public string str;
        }

        public struct MV_XML_FEATURE_Enumeration
        {
            public MV_XML_Visibility enVisivility;                              // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;                                              // ???
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;                                           // ??

            public Int32 nSymbolicNum;                           // Symbolic?
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_SYMBOLIC_STRLEN_C)]
            public string strCurrentSymbolic;                                   // ??Symbolic??
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_XML_SYMBOLIC_NUM)]
            public StrSymbolic[] strSymbolic;
            public MV_XML_AccessMode enAccessMode;                           // ????
            public Int32 bIsLocked;                              // ?????0-?;1-?
            public Int64 nValue;                                 // ???

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                          // ????
        }

        public struct MV_XML_FEATURE_Port
        {
            public MV_XML_Visibility enVisivility;                              // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;                                       // ????
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;                                              // ???
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;                                           // ??
            public MV_XML_AccessMode enAccessMode;                              // ????
            public Int32 bIsLocked;                                             // ?????0-?;1-?

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;                                          // ????
        }
        #endregion

        public enum MvGvspPixelType
        {
            // Undefined pixel type
            PixelType_Gvsp_Undefined = -1,

            // Mono buffer format defines
            PixelType_Gvsp_Mono1p = 0x01010037,
            PixelType_Gvsp_Mono2p = 0x01020038,
            PixelType_Gvsp_Mono4p = 0x01040039,
            PixelType_Gvsp_Mono8 = 0x01080001,
            PixelType_Gvsp_Mono8_Signed = 0x01080002,
            PixelType_Gvsp_Mono10 = 0x01100003,
            PixelType_Gvsp_Mono10_Packed = 0x010c0004,
            PixelType_Gvsp_Mono12 = 0x01100005,
            PixelType_Gvsp_Mono12_Packed = 0x010c0006,
            PixelType_Gvsp_Mono14 = 0x01100025,
            PixelType_Gvsp_Mono16 = 0x01100007,

            // Bayer buffer format defines 
            PixelType_Gvsp_BayerGR8 = 0x01080008,
            PixelType_Gvsp_BayerRG8 = 0x01080009,
            PixelType_Gvsp_BayerGB8 = 0x0108000a,
            PixelType_Gvsp_BayerBG8 = 0x0108000b,
            PixelType_Gvsp_BayerGR10 = 0x0110000c,
            PixelType_Gvsp_BayerRG10 = 0x0110000d,
            PixelType_Gvsp_BayerGB10 = 0x0110000e,
            PixelType_Gvsp_BayerBG10 = 0x0110000f,
            PixelType_Gvsp_BayerGR12 = 0x01100010,
            PixelType_Gvsp_BayerRG12 = 0x01100011,
            PixelType_Gvsp_BayerGB12 = 0x01100012,
            PixelType_Gvsp_BayerBG12 = 0x01100013,
            PixelType_Gvsp_BayerGR10_Packed = 0x010c0026,
            PixelType_Gvsp_BayerRG10_Packed = 0x010c0027,
            PixelType_Gvsp_BayerGB10_Packed = 0x010c0028,
            PixelType_Gvsp_BayerBG10_Packed = 0x010c0029,
            PixelType_Gvsp_BayerGR12_Packed = 0x010c002a,
            PixelType_Gvsp_BayerRG12_Packed = 0x010c002b,
            PixelType_Gvsp_BayerGB12_Packed = 0x010c002c,
            PixelType_Gvsp_BayerBG12_Packed = 0x010c002d,
            PixelType_Gvsp_BayerGR16 = 0x0110002e,
            PixelType_Gvsp_BayerRG16 = 0x0110002f,
            PixelType_Gvsp_BayerGB16 = 0x01100030,
            PixelType_Gvsp_BayerBG16 = 0x01100031,

            // RGB Packed buffer format defines 
            PixelType_Gvsp_RGB8_Packed = 0x02180014,
            PixelType_Gvsp_BGR8_Packed = 0x02180015,
            PixelType_Gvsp_RGBA8_Packed = 0x02200016,
            PixelType_Gvsp_BGRA8_Packed = 0x02200017,
            PixelType_Gvsp_RGB10_Packed = 0x02300018,
            PixelType_Gvsp_BGR10_Packed = 0x02300019,
            PixelType_Gvsp_RGB12_Packed = 0x0230001a,
            PixelType_Gvsp_BGR12_Packed = 0x0230001b,
            PixelType_Gvsp_RGB16_Packed = 0x02300033,
            PixelType_Gvsp_RGB10V1_Packed = 0x0220001c,
            PixelType_Gvsp_RGB10V2_Packed = 0x0220001d,
            PixelType_Gvsp_RGB12V1_Packed = 0x02240034,
            PixelType_Gvsp_RGB565_Packed = 0x02100035,
            PixelType_Gvsp_BGR565_Packed = 0x02100036,

            // YUV Packed buffer format defines 
            PixelType_Gvsp_YUV411_Packed = 0x020c001e,
            PixelType_Gvsp_YUV422_Packed = 0x0210001f,
            PixelType_Gvsp_YUV422_YUYV_Packed = 0x02100032,
            PixelType_Gvsp_YUV444_Packed = 0x02180020,
            PixelType_Gvsp_YCBCR8_CBYCR = 0x0218003a,
            PixelType_Gvsp_YCBCR422_8 = 0x0210003b,
            PixelType_Gvsp_YCBCR422_8_CBYCRY = 0x02100043,
            PixelType_Gvsp_YCBCR411_8_CBYYCRYY = 0x020c003c,
            PixelType_Gvsp_YCBCR601_8_CBYCR = 0x0218003d,
            PixelType_Gvsp_YCBCR601_422_8 = 0x0210003e,
            PixelType_Gvsp_YCBCR601_422_8_CBYCRY = 0x02100044,
            PixelType_Gvsp_YCBCR601_411_8_CBYYCRYY = 0x020c003f,
            PixelType_Gvsp_YCBCR709_8_CBYCR = 0x02180040,
            PixelType_Gvsp_YCBCR709_422_8 = 0x02100041,
            PixelType_Gvsp_YCBCR709_422_8_CBYCRY = 0x02100045,
            PixelType_Gvsp_YCBCR709_411_8_CBYYCRYY = 0x020c0042,

            // RGB Planar buffer format defines 
            PixelType_Gvsp_RGB8_Planar = 0x02180021,
            PixelType_Gvsp_RGB10_Planar = 0x02300022,
            PixelType_Gvsp_RGB12_Planar = 0x02300023,
            PixelType_Gvsp_RGB16_Planar = 0x02300024,

            // ????????
            PixelType_Gvsp_Jpeg = unchecked((Int32)0x80180001),

            PixelType_Gvsp_Coord3D_ABC32f = 0x026000C0,
            PixelType_Gvsp_Coord3D_ABC32f_Planar = 0x026000C1,


            PixelType_Gvsp_Coord3D_AC32f = 0x024000C2,//3D coordinate A-C 32-bit floating point
            PixelType_Gvsp_COORD3D_DEPTH_PLUS_MASK = unchecked((Int32)0x821c0001),

            PixelType_Gvsp_Coord3D_ABC32 = unchecked((Int32)0x82603001),

            PixelType_Gvsp_Coord3D_AB32f = unchecked((Int32)0x82403002),
            PixelType_Gvsp_Coord3D_AB32 = unchecked((Int32)0x82403003),

            PixelType_Gvsp_Coord3D_AC32f_Planar = 0x024000C3,
            PixelType_Gvsp_Coord3D_AC32 = unchecked((Int32)0x82403004),

            PixelType_Gvsp_Coord3D_A32f = 0x012000BD,
            PixelType_Gvsp_Coord3D_A32 = unchecked((Int32)0x81203005),

            PixelType_Gvsp_Coord3D_C32f = 0x012000BF,
            PixelType_Gvsp_Coord3D_C32 = unchecked((Int32)0x81203006),

            PixelType_Gvsp_Coord3D_ABC16 = 0x023000b9,
            PixelType_Gvsp_Coord3D_C16 = 0x011000b8,

            //??????????
            PixelType_Gvsp_HB_Mono8 = unchecked((Int32)0x81080001),
            PixelType_Gvsp_HB_Mono10 = unchecked((Int32)0x81100003),
            PixelType_Gvsp_HB_Mono10_Packed = unchecked((Int32)0x810c0004),
            PixelType_Gvsp_HB_Mono12 = unchecked((Int32)0x81100005),
            PixelType_Gvsp_HB_Mono12_Packed = unchecked((Int32)0x810c0006),
            PixelType_Gvsp_HB_Mono16 = unchecked((Int32)0x81100007),
            PixelType_Gvsp_HB_BayerGR8 = unchecked((Int32)0x81080008),
            PixelType_Gvsp_HB_BayerRG8 = unchecked((Int32)0x81080009),
            PixelType_Gvsp_HB_BayerGB8 = unchecked((Int32)0x8108000a),
            PixelType_Gvsp_HB_BayerBG8 = unchecked((Int32)0x8108000b),
            PixelType_Gvsp_HB_BayerGR10 = unchecked((Int32)0x8110000c),
            PixelType_Gvsp_HB_BayerRG10 = unchecked((Int32)0x8110000d),
            PixelType_Gvsp_HB_BayerGB10 = unchecked((Int32)0x8110000e),
            PixelType_Gvsp_HB_BayerBG10 = unchecked((Int32)0x8110000f),
            PixelType_Gvsp_HB_BayerGR12 = unchecked((Int32)0x81100010),
            PixelType_Gvsp_HB_BayerRG12 = unchecked((Int32)0x81100011),
            PixelType_Gvsp_HB_BayerGB12 = unchecked((Int32)0x81100012),
            PixelType_Gvsp_HB_BayerBG12 = unchecked((Int32)0x81100013),
            PixelType_Gvsp_HB_BayerGR10_Packed = unchecked((Int32)0x810c0026),
            PixelType_Gvsp_HB_BayerRG10_Packed = unchecked((Int32)0x810c0027),
            PixelType_Gvsp_HB_BayerGB10_Packed = unchecked((Int32)0x810c0028),
            PixelType_Gvsp_HB_BayerBG10_Packed = unchecked((Int32)0x810c0029),
            PixelType_Gvsp_HB_BayerGR12_Packed = unchecked((Int32)0x810c002a),
            PixelType_Gvsp_HB_BayerRG12_Packed = unchecked((Int32)0x810c002b),
            PixelType_Gvsp_HB_BayerGB12_Packed = unchecked((Int32)0x810c002c),
            PixelType_Gvsp_HB_BayerBG12_Packed = unchecked((Int32)0x810c002d),
            PixelType_Gvsp_HB_YUV422_Packed = unchecked((Int32)0x8210001f),
            PixelType_Gvsp_HB_YUV422_YUYV_Packed = unchecked((Int32)0x82100032),
            PixelType_Gvsp_HB_RGB8_Packed = unchecked((Int32)0x82180014),
            PixelType_Gvsp_HB_BGR8_Packed = unchecked((Int32)0x82180015),
            PixelType_Gvsp_HB_RGBA8_Packed = unchecked((Int32)0x82200016),
            PixelType_Gvsp_HB_BGRA8_Packed = unchecked((Int32)0x82200017),
        };

        // ??????
        IntPtr handle;                                                          // ????

        #region ?C/C++????????
        /************************************************************************/
        /* ??????????                                         */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetSDKVersion")]
        private static extern UInt32 MV_CC_GetSDKVersion();

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumerateTls")]
        private static extern Int32 MV_CC_EnumerateTls();

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumDevices")]
        private static extern Int32 MV_CC_EnumDevices(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumDevicesEx")]
        private static extern Int32 MV_CC_EnumDevicesEx(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList, String pManufacturerName);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_IsDeviceAccessible")]
        private static extern Boolean MV_CC_IsDeviceAccessible(ref MV_CC_DEVICE_INFO stDevInfo, UInt32 nAccessMode);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetSDKLogPath")]
        private static extern Int32 MV_CC_SetSDKLogPath(String pSDKLogPath);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CreateHandle")]
        private static extern Int32 MV_CC_CreateHandle(ref IntPtr handle, ref MV_CC_DEVICE_INFO stDevInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CreateHandleWithoutLog")]
        private static extern Int32 MV_CC_CreateHandleWithoutLog(ref IntPtr handle, ref MV_CC_DEVICE_INFO stDevInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DestroyHandle")]
        private static extern Int32 MV_CC_DestroyHandle(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_OpenDevice")]
        private static extern Int32 MV_CC_OpenDevice(IntPtr handle, UInt32 nAccessMode, UInt16 nSwitchoverKey);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CloseDevice")]
        private static extern Int32 MV_CC_CloseDevice(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_IsDeviceConnected")]
        private static extern Boolean MV_CC_IsDeviceConnected(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterImageCallBackEx")]
        private static extern Int32 MV_CC_RegisterImageCallBackEx(IntPtr handle, cbOutputExdelegate cbOutput, IntPtr pUser);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterImageCallBackForRGB")]
        private static extern Int32 MV_CC_RegisterImageCallBackForRGB(IntPtr handle, cbOutputExdelegate cbOutput, IntPtr pUser);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterImageCallBackForBGR")]
        private static extern Int32 MV_CC_RegisterImageCallBackForBGR(IntPtr handle, cbOutputExdelegate cbOutput, IntPtr pUser);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_StartGrabbing")]
        private static extern Int32 MV_CC_StartGrabbing(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_StopGrabbing")]
        private static extern Int32 MV_CC_StopGrabbing(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetImageForRGB")]
        private static extern Int32 MV_CC_GetImageForRGB(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetImageForBGR")]
        private static extern Int32 MV_CC_GetImageForBGR(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetImageBuffer")]
        private static extern Int32 MV_CC_GetImageBuffer(IntPtr handle, ref MV_FRAME_OUT pFrame, Int32 nMsec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FreeImageBuffer")]
        private static extern Int32 MV_CC_FreeImageBuffer(IntPtr handle, ref MV_FRAME_OUT pFrame);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetOneFrameTimeout")]
        private static extern Int32 MV_CC_GetOneFrameTimeout(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ClearImageBuffer")]
        private static extern Int32 MV_CC_ClearImageBuffer(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_Display")]
        private static extern Int32 MV_CC_Display(IntPtr handle, IntPtr hWnd);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DisplayOneFrame")]
        private static extern Int32 MV_CC_DisplayOneFrame(IntPtr handle, ref MV_DISPLAY_FRAME_INFO pDisplayInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetImageNodeNum")]
        private static extern Int32 MV_CC_SetImageNodeNum(IntPtr handle, UInt32 nNum);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGrabStrategy")]
        private static extern Int32 MV_CC_SetGrabStrategy(IntPtr handle, MV_GRAB_STRATEGY enGrabStrategy);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetOutputQueueSize")]
        private static extern Int32 MV_CC_SetOutputQueueSize(IntPtr handle, UInt32 nOutputQueueSize);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetImageInfo")]
        private static extern Int32 MV_CC_GetImageInfo(IntPtr handle, ref MV_IMAGE_BASIC_INFO pstInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetDeviceInfo")]
        private static extern Int32 MV_CC_GetDeviceInfo(IntPtr handle, ref MV_CC_DEVICE_INFO pstDevInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAllMatchInfo")]
        private static extern Int32 MV_CC_GetAllMatchInfo(IntPtr handle, ref MV_ALL_MATCH_INFO pstInfo);


        /************************************************************************/
        /* ??????????????                                 */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetIntValue")]
        private static extern Int32 MV_CC_GetIntValue(IntPtr handle, String strValue, ref MVCC_INTVALUE pIntValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetIntValueEx")]
        private static extern Int32 MV_CC_GetIntValueEx(IntPtr handle, String strValue, ref MVCC_INTVALUE_EX pIntValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetIntValue")]
        private static extern Int32 MV_CC_SetIntValue(IntPtr handle, String strValue, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetIntValueEx")]
        private static extern Int32 MV_CC_SetIntValueEx(IntPtr handle, String strValue, Int64 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetEnumValue")]
        private static extern Int32 MV_CC_GetEnumValue(IntPtr handle, String strValue, ref MVCC_ENUMVALUE pEnumValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetEnumValue")]
        private static extern Int32 MV_CC_SetEnumValue(IntPtr handle, String strValue, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetEnumValueByString")]
        private static extern Int32 MV_CC_SetEnumValueByString(IntPtr handle, String strValue, String sValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetFloatValue")]
        private static extern Int32 MV_CC_GetFloatValue(IntPtr handle, String strValue, ref MVCC_FLOATVALUE pFloatValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetFloatValue")]
        private static extern Int32 MV_CC_SetFloatValue(IntPtr handle, String strValue, Single fValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBoolValue")]
        private static extern Int32 MV_CC_GetBoolValue(IntPtr handle, String strValue, ref Boolean pBoolValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBoolValue")]
        private static extern Int32 MV_CC_SetBoolValue(IntPtr handle, String strValue, Boolean bValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetStringValue")]
        private static extern Int32 MV_CC_GetStringValue(IntPtr handle, String strKey, ref MVCC_STRINGVALUE pStringValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetStringValue")]
        private static extern Int32 MV_CC_SetStringValue(IntPtr handle, String strKey, String sValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetCommandValue")]
        private static extern Int32 MV_CC_SetCommandValue(IntPtr handle, String strValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_InvalidateNodes")]
        private static extern Int32 MV_CC_InvalidateNodes(IntPtr handle);

        /************************************************************************/
        /* ?????????,????????,?????,????????????   */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetWidth")]
        private static extern Int32 MV_CC_GetWidth(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetWidth")]
        private static extern Int32 MV_CC_SetWidth(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetHeight")]
        private static extern Int32 MV_CC_GetHeight(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetHeight")]
        private static extern Int32 MV_CC_SetHeight(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAOIoffsetX")]
        private static extern Int32 MV_CC_GetAOIoffsetX(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAOIoffsetX")]
        private static extern Int32 MV_CC_SetAOIoffsetX(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAOIoffsetY")]
        private static extern Int32 MV_CC_GetAOIoffsetY(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAOIoffsetY")]
        private static extern Int32 MV_CC_SetAOIoffsetY(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAutoExposureTimeLower")]
        private static extern Int32 MV_CC_GetAutoExposureTimeLower(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAutoExposureTimeLower")]
        private static extern Int32 MV_CC_SetAutoExposureTimeLower(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAutoExposureTimeUpper")]
        private static extern Int32 MV_CC_GetAutoExposureTimeUpper(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAutoExposureTimeUpper")]
        private static extern Int32 MV_CC_SetAutoExposureTimeUpper(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBrightness")]
        private static extern Int32 MV_CC_GetBrightness(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBrightness")]
        private static extern Int32 MV_CC_SetBrightness(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetFrameRate")]
        private static extern Int32 MV_CC_GetFrameRate(IntPtr handle, ref MVCC_FLOATVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetFrameRate")]
        private static extern Int32 MV_CC_SetFrameRate(IntPtr handle, Single fValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetGain")]
        private static extern Int32 MV_CC_GetGain(IntPtr handle, ref MVCC_FLOATVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGain")]
        private static extern Int32 MV_CC_SetGain(IntPtr handle, Single fValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetExposureTime")]
        private static extern Int32 MV_CC_GetExposureTime(IntPtr handle, ref MVCC_FLOATVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetExposureTime")]
        private static extern Int32 MV_CC_SetExposureTime(IntPtr handle, Single fValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetPixelFormat")]
        private static extern Int32 MV_CC_GetPixelFormat(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetPixelFormat")]
        private static extern Int32 MV_CC_SetPixelFormat(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAcquisitionMode")]
        private static extern Int32 MV_CC_GetAcquisitionMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAcquisitionMode")]
        private static extern Int32 MV_CC_SetAcquisitionMode(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetGainMode")]
        private static extern Int32 MV_CC_GetGainMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGainMode")]
        private static extern Int32 MV_CC_SetGainMode(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetExposureAutoMode")]
        private static extern Int32 MV_CC_GetExposureAutoMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetExposureAutoMode")]
        private static extern Int32 MV_CC_SetExposureAutoMode(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetTriggerMode")]
        private static extern Int32 MV_CC_GetTriggerMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetTriggerMode")]
        private static extern Int32 MV_CC_SetTriggerMode(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetTriggerDelay")]
        private static extern Int32 MV_CC_GetTriggerDelay(IntPtr handle, ref MVCC_FLOATVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetTriggerDelay")]
        private static extern Int32 MV_CC_SetTriggerDelay(IntPtr handle, Single fValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetTriggerSource")]
        private static extern Int32 MV_CC_GetTriggerSource(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetTriggerSource")]
        private static extern Int32 MV_CC_SetTriggerSource(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_TriggerSoftwareExecute")]
        private static extern Int32 MV_CC_TriggerSoftwareExecute(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetGammaSelector")]
        private static extern Int32 MV_CC_GetGammaSelector(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGammaSelector")]
        private static extern Int32 MV_CC_SetGammaSelector(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetGamma")]
        private static extern Int32 MV_CC_GetGamma(IntPtr handle, ref MVCC_FLOATVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGamma")]
        private static extern Int32 MV_CC_SetGamma(IntPtr handle, Single fValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetSharpness")]
        private static extern Int32 MV_CC_GetSharpness(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetSharpness")]
        private static extern Int32 MV_CC_SetSharpness(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetHue")]
        private static extern Int32 MV_CC_GetHue(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetHue")]
        private static extern Int32 MV_CC_SetHue(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetSaturation")]
        private static extern Int32 MV_CC_GetSaturation(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetSaturation")]
        private static extern Int32 MV_CC_SetSaturation(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBalanceWhiteAuto")]
        private static extern Int32 MV_CC_GetBalanceWhiteAuto(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBalanceWhiteAuto")]
        private static extern Int32 MV_CC_SetBalanceWhiteAuto(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBalanceRatioRed")]
        private static extern Int32 MV_CC_GetBalanceRatioRed(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBalanceRatioRed")]
        private static extern Int32 MV_CC_SetBalanceRatioRed(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBalanceRatioGreen")]
        private static extern Int32 MV_CC_GetBalanceRatioGreen(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBalanceRatioGreen")]
        private static extern Int32 MV_CC_SetBalanceRatioGreen(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBalanceRatioBlue")]
        private static extern Int32 MV_CC_GetBalanceRatioBlue(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBalanceRatioBlue")]
        private static extern Int32 MV_CC_SetBalanceRatioBlue(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetDeviceUserID")]
        private static extern Int32 MV_CC_GetDeviceUserID(IntPtr handle, ref MVCC_STRINGVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetDeviceUserID")]
        private static extern Int32 MV_CC_SetDeviceUserID(IntPtr handle, string chValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBurstFrameCount")]
        private static extern Int32 MV_CC_GetBurstFrameCount(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBurstFrameCount")]
        private static extern Int32 MV_CC_SetBurstFrameCount(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAcquisitionLineRate")]
        private static extern Int32 MV_CC_GetAcquisitionLineRate(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAcquisitionLineRate")]
        private static extern Int32 MV_CC_SetAcquisitionLineRate(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetHeartBeatTimeout")]
        private static extern Int32 MV_CC_GetHeartBeatTimeout(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetHeartBeatTimeout")]
        private static extern Int32 MV_CC_SetHeartBeatTimeout(IntPtr handle, UInt32 nValue);

        /************************************************************************/
        /* ???? ? ????? ????????                            */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_LocalUpgrade")]
        private static extern Int32 MV_CC_LocalUpgrade(IntPtr handle, String pFilePathName);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetUpgradeProcess")]
        private static extern Int32 MV_CC_GetUpgradeProcess(IntPtr handle, ref UInt32 pnProcess);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetOptimalPacketSize")]
        private static extern Int32 MV_CC_GetOptimalPacketSize(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ReadMemory")]
        private static extern Int32 MV_CC_ReadMemory(IntPtr handle, IntPtr pBuffer, Int64 nAddress, Int64 nLength);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_WriteMemory")]
        private static extern Int32 MV_CC_WriteMemory(IntPtr handle, IntPtr pBuffer, Int64 nAddress, Int64 nLength);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterExceptionCallBack")]
        private static extern Int32 MV_CC_RegisterExceptionCallBack(IntPtr handle, cbExceptiondelegate cbException, IntPtr pUser);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterEventCallBack")]
        private static extern Int32 MV_CC_RegisterEventCallBack(IntPtr handle, cbEventdelegate cbEvent, IntPtr pUser);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterAllEventCallBack")]
        private static extern Int32 MV_CC_RegisterAllEventCallBack(IntPtr handle, cbEventdelegateEx cbEvent, IntPtr pUser);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterEventCallBackEx")]
        private static extern Int32 MV_CC_RegisterEventCallBackEx(IntPtr handle, String pEventName, cbEventdelegateEx cbEvent, IntPtr pUser);

        /************************************************************************/
        /* GigEVision ???????                                     */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_ForceIpEx")]
        private static extern Int32 MV_GIGE_ForceIpEx(IntPtr handle, UInt32 nIP, UInt32 nSubNetMask, UInt32 nDefaultGateWay);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetIpConfig")]
        private static extern Int32 MV_GIGE_SetIpConfig(IntPtr handle, UInt32 nType);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetNetTransMode")]
        private static extern Int32 MV_GIGE_SetNetTransMode(IntPtr handle, UInt32 nType);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetNetTransInfo")]
        private static extern Int32 MV_GIGE_GetNetTransInfo(IntPtr handle, ref MV_NETTRANS_INFO pstInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetDiscoveryMode")]
        private static extern Int32 MV_GIGE_SetDiscoveryMode(UInt32 nMode);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGvspTimeout")]
        private static extern Int32 MV_GIGE_SetGvspTimeout(IntPtr handle, UInt32 nMillisec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGvspTimeout")]
        private static extern Int32 MV_GIGE_GetGvspTimeout(IntPtr handle, ref UInt32 pMillisec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGvcpTimeout")]
        private static extern Int32 MV_GIGE_SetGvcpTimeout(IntPtr handle, UInt32 nMillisec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGvcpTimeout")]
        private static extern Int32 MV_GIGE_GetGvcpTimeout(IntPtr handle, ref UInt32 pMillisec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetRetryGvcpTimes")]
        private static extern Int32 MV_GIGE_SetRetryGvcpTimes(IntPtr handle, UInt32 nRetryGvcpTimes);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetRetryGvcpTimes")]
        private static extern Int32 MV_GIGE_GetRetryGvcpTimes(IntPtr handle, ref UInt32 pRetryGvcpTimes);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetResend")]
        private static extern Int32 MV_GIGE_SetResend(IntPtr handle, UInt32 bEnable, UInt32 nMaxResendPercent, UInt32 nResendTimeout);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetResendMaxRetryTimes")]
        private static extern Int32 MV_GIGE_SetResendMaxRetryTimes(IntPtr handle, UInt32 nRetryTimes);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetResendMaxRetryTimes")]
        private static extern Int32 MV_GIGE_GetResendMaxRetryTimes(IntPtr handle, ref UInt32 pnRetryTimes);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetResendTimeInterval")]
        private static extern Int32 MV_GIGE_SetResendTimeInterval(IntPtr handle, UInt32 nMillisec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetResendTimeInterval")]
        private static extern Int32 MV_GIGE_GetResendTimeInterval(IntPtr handle, ref UInt32 pnMillisec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGevSCPSPacketSize")]
        private static extern Int32 MV_GIGE_GetGevSCPSPacketSize(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGevSCPSPacketSize")]
        private static extern Int32 MV_GIGE_SetGevSCPSPacketSize(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGevSCPD")]
        private static extern Int32 MV_GIGE_GetGevSCPD(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGevSCPD")]
        private static extern Int32 MV_GIGE_SetGevSCPD(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGevSCDA")]
        private static extern Int32 MV_GIGE_GetGevSCDA(IntPtr handle, ref UInt32 pnIP);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGevSCDA")]
        private static extern Int32 MV_GIGE_SetGevSCDA(IntPtr handle, UInt32 nIP);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGevSCSP")]
        private static extern Int32 MV_GIGE_GetGevSCSP(IntPtr handle, ref UInt32 pnPort);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGevSCSP")]
        private static extern Int32 MV_GIGE_SetGevSCSP(IntPtr handle, UInt32 nPort);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetTransmissionType")]
        private static extern Int32 MV_GIGE_SetTransmissionType(IntPtr handle, ref MV_CC_TRANSMISSION_TYPE pstTransmissionType);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_IssueActionCommand")]
        private static extern Int32 MV_GIGE_IssueActionCommand(ref MV_ACTION_CMD_INFO pstActionCmdInfo, ref MV_ACTION_CMD_RESULT_LIST pstActionCmdResults);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetMulticastStatus")]
        private static extern Int32 MV_GIGE_GetMulticastStatus(ref MV_CC_DEVICE_INFO pstDevInfo, ref Boolean pStatus);


        //CameraLink?????
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CAML_SetDeviceBauderate")]
        private static extern Int32 MV_CAML_SetDeviceBaudrate(IntPtr handle, UInt32 nBaudrate);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CAML_GetDeviceBauderate")]
        private static extern Int32 MV_CAML_GetDeviceBaudrate(IntPtr handle, ref UInt32 pnCurrentBaudrate);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CAML_GetSupportBauderates")]
        private static extern Int32 MV_CAML_GetSupportBaudrates(IntPtr handle, ref UInt32 pnBaudrateAblity);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CAML_SetGenCPTimeOut")]
        private static extern Int32 MV_CAML_SetGenCPTimeOut(IntPtr handle, UInt32 nMillisec);

        /************************************************************************/
        /* U3V ???????                                            */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_SetTransferSize")]
        private static extern Int32 MV_USB_SetTransferSize(IntPtr handle, UInt32 nTransferSize);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_GetTransferSize")]
        private static extern Int32 MV_USB_GetTransferSize(IntPtr handle, ref UInt32 pTransferSize);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_SetTransferWays")]
        private static extern Int32 MV_USB_SetTransferWays(IntPtr handle, UInt32 nTransferWays);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_GetTransferWays")]
        private static extern Int32 MV_USB_GetTransferWays(IntPtr handle, ref UInt32 pTransferWays);


        /************************************************************************/
        /* GenTL????,????????(???????)                    */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumInterfacesByGenTL")]
        private static extern Int32 MV_CC_EnumInterfacesByGenTL(ref MV_GENTL_IF_INFO_LIST pstIFInfoList, String sGenTLPath);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumDevicesByGenTL")]
        private static extern Int32 MV_CC_EnumDevicesByGenTL(ref MV_GENTL_IF_INFO stIFInfo, ref MV_GENTL_DEV_INFO_LIST pstDevList);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CreateHandleByGenTL")]
        private static extern Int32 MV_CC_CreateHandleByGenTL(ref IntPtr handle, ref MV_GENTL_DEV_INFO stDevInfo);

        /************************************************************************/
        /* XML??????                                                         */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetGenICamXML")]
        private static extern Int32 MV_XML_GetGenICamXML(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref UInt32 pnDataLen);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetNodeAccessMode")]
        private static extern Int32 MV_XML_GetNodeAccessMode(IntPtr handle, String pstrName, ref MV_XML_AccessMode pAccessMode);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetNodeInterfaceType")]
        private static extern Int32 MV_XML_GetNodeInterfaceType(IntPtr handle, String pstrName, ref MV_XML_InterfaceType pInterfaceType);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetRootNode")]
        private static extern Int32 MV_XML_GetRootNode(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetChildren")]
        private static extern Int32 MV_XML_GetChildren(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode, IntPtr pstNodesList);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetChildren")]
        private static extern Int32 MV_XML_GetChildren(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode, ref MV_XML_NODES_LIST pstNodesList);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetNodeFeature")]
        private static extern Int32 MV_XML_GetNodeFeature(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode, IntPtr pstFeature);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_UpdateNodeFeature")]
        private static extern Int32 MV_XML_UpdateNodeFeature(IntPtr handle, MV_XML_InterfaceType enType, IntPtr pstFeature);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_RegisterUpdateCallBack")]
        private static extern Int32 MV_XML_RegisterUpdateCallBack(IntPtr handle, cbXmlUpdatedelegate cbXmlUpdate, IntPtr pUser);

        /************************************************************************/
        /* ????                                   */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SaveImageEx2")]
        private static extern Int32 MV_CC_SaveImageEx2(IntPtr handle, ref MV_SAVE_IMAGE_PARAM_EX stSaveParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ConvertPixelType")]
        private static extern Int32 MV_CC_ConvertPixelType(IntPtr handle, ref MV_PIXEL_CONVERT_PARAM pstCvtParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerCvtQuality")]
        private static extern Int32 MV_CC_SetBayerCvtQuality(IntPtr handle, UInt32 BayerCvtQuality);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerGammaValue")]
        private static extern Int32 MV_CC_SetBayerGammaValue(IntPtr handle, Single fBayerGammaValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerGammaParam")]
        private static extern Int32 MV_CC_SetBayerGammaParam(IntPtr handle, ref MV_CC_GAMMA_PARAM pstGammaParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerCCMParam")]
        private static extern Int32 MV_CC_SetBayerCCMParam(IntPtr handle, ref MV_CC_CCM_PARAM pstCCMParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerCCMParamEx")]
        private static extern Int32 MV_CC_SetBayerCCMParamEx(IntPtr handle, ref MV_CC_CCM_PARAM_EX pstCCMParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerCLUTParam")]
        private static extern Int32 MV_CC_SetBayerCLUTParam(IntPtr handle, ref MV_CC_CLUT_PARAM pstCLUTParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ImageContrast")]
        private static extern Int32 MV_CC_ImageContrast(IntPtr handle, ref MV_CC_CONTRAST_PARAM pstContrastParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ImageSharpen")]
        private static extern Int32 MV_CC_ImageSharpen(IntPtr handle, ref MV_CC_SHARPEN_PARAM pstSharpenParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ColorCorrect")]
        private static extern Int32 MV_CC_ColorCorrect(IntPtr handle, ref MV_CC_COLOR_CORRECT_PARAM pstColorCorrectParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_NoiseEstimate")]
        private static extern Int32 MV_CC_NoiseEstimate(IntPtr handle, ref MV_CC_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SpatialDenoise")]
        private static extern Int32 MV_CC_SpatialDenoise(IntPtr handle, ref MV_CC_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_LSCCalib")]
        private static extern Int32 MV_CC_LSCCalib(IntPtr handle, ref MV_CC_LSC_CALIB_PARAM pstLSCCalibParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_LSCCorrect")]
        private static extern Int32 MV_CC_LSCCorrect(IntPtr handle, ref MV_CC_LSC_CORRECT_PARAM pstLSCCorrectParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_HB_Decode")]
        private static extern Int32 MV_CC_HB_Decode(IntPtr handle, ref MV_CC_HB_DECODE_PARAM pstDecodeParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetTlProxy")]
        private static extern IntPtr MV_CC_GetTlProxy(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FeatureSave")]
        private static extern Int32 MV_CC_FeatureSave(IntPtr handle, String pFileName);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FeatureLoad")]
        private static extern Int32 MV_CC_FeatureLoad(IntPtr handle, String pFileName);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FileAccessRead")]
        private static extern Int32 MV_CC_FileAccessRead(IntPtr handle, ref MV_CC_FILE_ACCESS pstFileAccess);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FileAccessWrite")]
        private static extern Int32 MV_CC_FileAccessWrite(IntPtr handle, ref MV_CC_FILE_ACCESS pstFileAccess);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetFileAccessProgress")]
        private static extern Int32 MV_CC_GetFileAccessProgress(IntPtr handle, ref MV_CC_FILE_ACCESS_PROGRESS pstFileAccessProgress);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_StartRecord")]
        private static extern Int32 MV_CC_StartRecord(IntPtr handle, ref MV_CC_RECORD_PARAM pstRecordParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_InputOneFrame")]
        private static extern Int32 MV_CC_InputOneFrame(IntPtr handle, ref MV_CC_INPUT_FRAME_INFO pstInputFrameInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_StopRecord")]
        private static extern Int32 MV_CC_StopRecord(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SaveImageToFile")]
        private static extern Int32 MV_CC_SaveImageToFile(IntPtr handle, ref MV_SAVE_IMG_TO_FILE_PARAM pstSaveFileParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SavePointCloudData")]
        private static extern Int32 MV_CC_SavePointCloudData(IntPtr handle, ref MV_SAVE_POINT_CLOUD_PARAM pstPointDataParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RotateImage")]
        private static extern Int32 MV_CC_RotateImage(IntPtr handle, ref MV_CC_ROTATE_IMAGE_PARAM pstRotateParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FlipImage")]
        private static extern Int32 MV_CC_FlipImage(IntPtr handle, ref MV_CC_FLIP_IMAGE_PARAM pstFlipParam);

        /************************************************************************/
        /* ?????                                 */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetOneFrame")]
        private static extern Int32 MV_CC_GetOneFrame(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO pFrameInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetOneFrameEx")]
        private static extern Int32 MV_CC_GetOneFrameEx(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterImageCallBack")]
        private static extern Int32 MV_CC_RegisterImageCallBack(IntPtr handle, cbOutputdelegate cbOutput, IntPtr pUser);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SaveImage")]
        private static extern Int32 MV_CC_SaveImage(ref MV_SAVE_IMAGE_PARAM stSaveParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_ForceIp")]
        private static extern Int32 MV_GIGE_ForceIp(IntPtr handle, UInt32 nIP);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_BayerNoiseEstimate")]
        private static extern Int32 MV_CC_BayerNoiseEstimate(IntPtr handle, ref MV_CC_BAYER_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_BayerSpatialDenoise")]
        private static extern Int32 MV_CC_BayerSpatialDenoise(IntPtr handle, ref MV_CC_BAYER_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_StartGrabbingEx")]
        private static extern Int32 MV_CC_StartGrabbingEx(IntPtr handle, UInt32 bNeedStart);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_StopGrabbingEx")]
        private static extern Int32 MV_CC_StopGrabbingEx(IntPtr handle, UInt32 bNeedStop);
        #endregion
    }
}
