using System.DirectoryServices.AccountManagement;

namespace PassReset.Commands;

public class ActiveDirectoryProvider
{
	private readonly PrincipalContext _principalContext;

	public ActiveDirectoryProvider(PrincipalContext principalContext)
	{
		_principalContext = principalContext;
	}

	public UserPrincipal FindUserByName(string userName)
	{
		return UserPrincipal.FindByIdentity(_principalContext, userName);
	}

	public bool ValidateUserCredentials(string userName, string password)
	{
		return _principalContext.ValidateCredentials(userName, password);
	}

	public IEnumerable<Principal> GetUserSecurityGroups(string userName)
	{
		var userPrincipal = UserPrincipal.FindByIdentity(_principalContext, userName);

		if (userPrincipal == null)
		{
			throw new InvalidOperationException("User does not exist.");
		}

		return userPrincipal.GetAuthorizationGroups().ToList();
	}
}