using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Saludimetro.Utilities
{
	public class PatientMessenger : ValueChangedMessage<PatientMessage>
	{
		public PatientMessenger(PatientMessage value):base(value)
		{
		}
	}
}

