using FakeItEasy;
using Microsoft.AspNetCore.SignalR.Client;
using Scd.ProjectX.Client.Messaging.Dispatchers;
using Scd.ProjectX.Client.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scd.ProjectX.Client.Tests.Dispatchers
{
    public class EventDispatcherTests
    {
        [Fact(Skip = "TODO")]
        public void EventDispatcher_ShouldThrowArgumentException_WhenConnectionISNUll()
        {
            // Arrange
            HubConnection fakeConnection = A.Fake<HubConnection>();
            // Act & Assert
            //ArgumentNullException argumentNullException = 
                //Assert.Throws<ArgumentNullException>(() => new TestEventDispatcher(fakeConnection, "RemoteProcedure"));
        }

        private class TestEventDispatcher : EventDispatcher<TestStub>
        {
            public TestEventDispatcher(HubConnection connection, string publishMethodName)
                : base(connection, publishMethodName)
            {
            }
            public override void Init()
            {
                // No-op for testing
            }
            public override void Publish(TestStub @event)
            {
                // No-op for testing
            }
        }
    }
}
