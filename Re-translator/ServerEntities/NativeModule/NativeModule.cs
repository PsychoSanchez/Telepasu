namespace Proxy.ServerEntities.NativeModule
{
    public class NativeModule: EntityManager
    {
        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            // ignore
        }

        protected override void Disconnected(object sender, MessageArgs e)
        {
            // ignore
        }
    }
}
