namespace Basler.pyloncore
{
    public enum EPixelType
    {
        /// <summary>
        /// Undefined pixel type.
        /// </summary>
        Undefined = -1,
        /// <summary>
        /// Mono 1 bit packed <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Mono1packed or Mono1p.
        /// </summary>
        Mono1packed = -2130640884,
        /// <summary>
        /// Mono 2 bit packed <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Mono2packed or Mono2p.
        /// </summary>
        Mono2packed = -2130575347,
        /// <summary>
        /// Mono 4 bit packed <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Mono4packed or Mono4p.
        /// </summary>
        Mono4packed = -2130444274,
        /// <summary>
        /// Mono 8 bit packed <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Mono8.
        /// </summary>
        Mono8 = 17301505,
        /// <summary>
        /// Mono 8 bit signed <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Mono8signed.
        /// </summary>
        Mono8signed = 17301506,
        /// <summary>
        /// Mono 10 bit <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Mono10.
        /// </summary>
        Mono10 = 17825795,
        /// <summary>
        /// Mono 10 bit packed <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Mono10packed. The memory layouts of PixelType_Mono10packed and PixelType_Mono10p are different.
        /// </summary>
        Mono10packed = 17563652,
        /// <summary>
        /// Mono 10 bit <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Mono10p. The memory layouts of PixelType_Mono10packed and PixelType_Mono10p are different.
        /// </summary>
        Mono10p = 17432646,
        /// <summary>
        /// Mono 12 bit <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Mono12.
        /// </summary>
        Mono12 = 17825797,
        /// <summary>
        /// Mono 12 bit packed <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Mono12packed. The memory layouts of PixelType_Mono12packed and PixelType_Mono12p are different.
        /// </summary>
        Mono12packed = 17563654,
        /// <summary>
        /// Mono 12 bit packed <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Mono12p. The memory layouts of PixelType_Mono12packed and PixelType_Mono12p are different.
        /// </summary>
        Mono12p = 17563719,
        /// <summary>
        /// Mono 16 bit <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Mono16.
        /// </summary>
        Mono16 = 17825799,
        /// <summary>
        /// Bayer Green Red 8 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerGR8.
        /// </summary>
        BayerGR8 = 17301512,
        /// <summary>
        /// Bayer Red Green 8 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerRG8.
        /// </summary>
        BayerRG8 = 17301513,
        /// <summary>
        /// Bayer Green Blue 8 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerGB8.
        /// </summary>
        BayerGB8 = 17301514,
        /// <summary>
        /// Bayer Blue Green 8 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerBG8.
        /// </summary>
        BayerBG8 = 17301515,
        /// <summary>
        /// Bayer Green Red 10 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerGR10.
        /// </summary>
        BayerGR10 = 17825804,
        /// <summary>
        /// Bayer Red Green 10 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerRG10.
        /// </summary>
        BayerRG10 = 17825805,
        /// <summary>
        /// Bayer Green Blue 10 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerGB10.
        /// </summary>
        BayerGB10 = 17825806,
        /// <summary>
        /// Bayer Blue Green 10 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerBG10.
        /// </summary>
        BayerBG10 = 17825807,
        /// <summary>
        /// Bayer Green Red 12 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerGR12.
        /// </summary>
        BayerGR12 = 17825808,
        /// <summary>
        /// Bayer Red Green 12 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerRG12.
        /// </summary>
        BayerRG12 = 17825809,
        /// <summary>
        /// Bayer Green Blue 12 bit <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerGB12.
        /// </summary>
        BayerGB12 = 17825810,
        /// <summary>
        /// Bayer Blue Green 12 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerBG12.
        /// </summary>
        BayerBG12 = 17825811,
        /// <summary>
        /// Red, Green, Blue 8 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting RGB8packed.
        /// </summary>
        RGB8packed = 35127316,
        /// <summary>
        /// Blue, Green, Red, 8 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BGR8packed or BGR8.
        /// </summary>
        BGR8packed = 35127317,
        /// <summary>
        /// Red, Green, Blue, Alpha 8 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting RGBA8packed.
        /// </summary>
        RGBA8packed = 35651606,
        /// <summary>
        /// Blue, Green, Red, Alpha 8 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BGRA8packed or BGRa8.
        /// </summary>
        BGRA8packed = 35651607,
        /// <summary>
        /// Red, Green, Blue 10 bit <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting RGB10packed.
        /// </summary>
        RGB10packed = 36700184,
        /// <summary>
        /// Blue, Green, Red 10 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BGR10packed or BGR10.
        /// </summary>
        BGR10packed = 36700185,
        /// <summary>
        /// Red, Green, Blue 12 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting RGB12packed.
        /// </summary>
        RGB12packed = 36700186,
        /// <summary>
        /// Blue, Green, Red, 12 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BGR12packed or BGR12.
        /// </summary>
        BGR12packed = 36700187,
        /// <summary>
        /// Red, Green, Blue 16 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting RGB16packed.
        /// </summary>
        RGB16packed = 36700211,
        /// <summary>
        /// BGR 10 bit packed (GigE Vision Specific). <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BGR10V1packed.
        /// </summary>
        BGR10V1packed = 35651612,
        /// <summary>
        /// BGR 10 bit packed (GigE Vision Specific). <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BGR10V2packed.
        /// </summary>
        BGR10V2packed = 35651613,
        /// <summary>
        /// YUV 411 8 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting YUV411packed.
        /// </summary>
        YUV411packed = 34340894,
        /// <summary>
        /// YUV 422 8 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting YUV422packed.
        /// </summary>
        YUV422packed = 34603039,
        /// <summary>
        /// Corresponds to the camera's PixelFormat enumeration setting YUV444packed.
        /// </summary>
        YUV444packed = 35127328,
        /// <summary>
        /// Red, Green, Blue 8 bit planar. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting RGB8planar.
        /// </summary>
        RGB8planar = 35127329,
        /// <summary>
        /// Red, Green, Blue 10 bit planar. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting RGB10planar.
        /// </summary>
        RGB10planar = 36700194,
        /// <summary>
        /// Red, Green, Blue 12 bit planar. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting RGB12planar.
        /// </summary>
        RGB12planar = 36700195,
        /// <summary>
        /// Red, Green, Blue 16 bit planar. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting RGB16planar.
        /// </summary>
        RGB16planar = 36700196,
        /// <summary>
        /// Y, U, V 8 bit planar.
        /// </summary>
        YUV444planar = -2112356284,
        /// <summary>
        /// Y, U, V 8 bit planar.
        /// </summary>
        YUV422planar = -2112880574,
        /// <summary>
        /// Y, U, V 8 bit planar.
        /// </summary>
        YUV420planar = -2113142720,
        /// <summary>
        /// Corresponds to the camera's PixelFormat enumeration setting YUV422_YUYV_Packed or YCbCr422_8.
        /// </summary>
        YUV422_YUYV_Packed = 34603058,
        /// <summary>
        /// Bayer Green Red 12 bit packed (GigE Vision Specific). <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerGR12Packed. The memory layouts of PixelType_BayerGR12Packed and PixelType_BayerGR12p are different.
        /// </summary>
        BayerGR12Packed = 17563690,
        /// <summary>
        /// Bayer Red Green 12 bit packed (GigE Vision Specific). <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerRG12Packed. The memory layouts of PixelType_BayerRG12Packed and PixelType_BayerRG12p are different.
        /// </summary>
        BayerRG12Packed = 17563691,
        /// <summary>
        /// Bayer Green Blue 12 bit packed (GigE Vision Specific). <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerGB12Packed. The memory layouts of PixelType_BayerGB12Packed and PixelType_BayerGB12p are different.
        /// </summary>
        BayerGB12Packed = 17563692,
        /// <summary>
        /// Bayer Blue Green 12 bit packed (GigE Vision Specific). <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerBG12Packed. The memory layouts of PixelType_BayerBG12Packed and PixelType_BayerBG12p are different.
        /// </summary>
        BayerBG12Packed = 17563693,
        /// <summary>
        /// Bayer Green Red 10p bit packed. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerGR10p.
        /// </summary>
        BayerGR10pp = 17432662,
        /// <summary>
        /// Bayer Red Green 10p bit packed. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerRG10p.
        /// </summary>
        BayerRG10pp = 17432664,
        /// <summary>
        /// Bayer Green Blue 10p bit packed. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerGB10p.
        /// </summary>
        BayerGB10pp = 17432660,
        /// <summary>
        /// Bayer Blue Green 10p bit packed. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerBG10p.
        /// </summary>
        BayerBG10pp = 17432658,
        /// <summary>
        /// Bayer Green Red 12 bit packed. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerGR12p. The memory layouts of PixelType_BayerGR12Packed and PixelType_BayerGR12p are different.
        /// </summary>
        BayerGR12p = 17563735,
        /// <summary>
        /// Bayer Red Green 12 bit packed. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerRG12p. The memory layouts of PixelType_BayerRG12Packed and PixelType_BayerRG12p are different.
        /// </summary>
        BayerRG12p = 17563737,
        /// <summary>
        /// Bayer Green Blue 12 bit packed. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerGB12p. The memory layouts of PixelType_BayerGB12Packed and PixelType_BayerGB12p are different.
        /// </summary>
        BayerGB12p = 17563733,
        /// <summary>
        /// Bayer Blue Green 12 bit packed. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerBG12p. The memory layouts of PixelType_BayerBG12Packed and PixelType_BayerBG12p are different.
        /// </summary>
        BayerBG12p = 17563731,
        /// <summary>
        /// Bayer Green Red 16 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerGR16.
        /// </summary>
        BayerGR16 = 17825838,
        /// <summary>
        /// Bayer Red Green 16 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerRG16.
        /// </summary>
        BayerRG16 = 17825839,
        /// <summary>
        /// Bayer Green Blue 16 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerGB16.
        /// </summary>
        BayerGB16 = 17825840,
        /// <summary>
        /// Bayer Blue Green 16 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting BayerBG16.
        /// </summary>
        BayerBG16 = 17825841,
        /// <summary>
        /// Corresponds to the camera's PixelFormat enumeration setting RGB12V1packed.
        /// </summary>
        RGB12V1packed = 35913780,
        /// <summary>
        /// Double floating point 64 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Double.
        /// </summary>
        Double = -2126511872,
        /// <summary>
        /// Confidence Values 8 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Confidence8.
        /// </summary>
        Confidence8 = 17301702,
        /// <summary>
        /// Confidence Values 16 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Confidence16.
        /// </summary>
        Confidence16 = 17825991,
        /// <summary>
        /// 3D Coordinates 8 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Coord3D_C8.
        /// </summary>
        Coord3D_C8 = 17301681,
        /// <summary>
        /// 3D Coordinates 16 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Coord3D_C16.
        /// </summary>
        Coord3D_C16 = 17825976,
        /// <summary>
        /// 3D Coordinates 32 bit float. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Coord3D_ABC32f.
        /// </summary>
        Coord3D_ABC32f = 39846080,
        /// <summary>
        /// Float 32 bit. <br/>
        /// Corresponds to the camera's PixelFormat enumeration setting Data32f.
        /// </summary>
        Data32f = 18874652,
    }
}
