using FirebaseAdmin;
using FirebaseAdmin.Auth;

public class UserDataLoader : BatchDataLoader<string, UserType>
{
    private readonly FirebaseAuth _firebaseAuth;
    public UserDataLoader(FirebaseApp firebaseApp, IBatchScheduler batchScheduler, DataLoaderOptions? options = null) : base(batchScheduler, options)
    {
        _firebaseAuth = FirebaseAuth.GetAuth(firebaseApp);
    }

    protected override async Task<IReadOnlyDictionary<string, UserType>> LoadBatchAsync(IReadOnlyList<string> userIds, CancellationToken cancellationToken)
    {
        var userIdentifiers = userIds.Select(x => new UidIdentifier(x)).ToList();

        GetUsersResult usersResult = await _firebaseAuth.GetUsersAsync(userIdentifiers);

        return usersResult.Users.Select(x => new UserType(){
            Id = x.Uid,
            UserName = x.DisplayName,
            PhotoUrl = x.PhotoUrl
        }).ToDictionary(u => u.Id);
    }
}