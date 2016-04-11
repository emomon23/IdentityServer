using Identifix.IdentityServer.Infrastructure;
using Rhino.Mocks;

namespace Identifix.IdentityServer.Tests.Controllers
{
    public class ControllerMocks
    {
        public ControllerMocks()
        {
            Context = MockRepository.GenerateMock<IApplicationContext>();
            State = MockRepository.GenerateMock<IStateManager>();
            Settings = MockRepository.GenerateMock<ISettingManager>();
            Context.Stub(mock => mock.Settings).Return(Settings);
            Context.Stub(mock => mock.State).Return(State);
        }

        public IApplicationContext Context { get; set; }

        public ISettingManager Settings { get; set; }
        
        public IStateManager State { get; set; } 
    }
}