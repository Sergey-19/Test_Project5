using EQX.Vision.LightController;

namespace EQX.Vision.UnitTest
{
    public class LightControllerTest
    {
        [Fact]
        public void HIKVisionLightControllerTest()
        {
            LightControllerMV lightController = new LightControllerMV(1, "Align Light", "COM2");
            lightController.Connect();

            Assert.True(lightController.IsConnected);

            if (lightController.IsConnected == false) return;

            lightController.SetLightStatus(0, true);

            lightController.SetLightStatus(0, true);
            Thread.Sleep(2000);
            lightController.SetLightLevel(0, 50);
            Thread.Sleep(2000);
            lightController.SetLightLevel(0, 255);
            Thread.Sleep(2000);
            lightController.SetLightStatus(0, false);
            Thread.Sleep(2000);

            lightController.Disconnect();
        }
    }
}
