namespace TetrioSharp
{
    /// <summary>
    /// A TETR.IO Tetra Channel API wrapper 
    /// </summary>
    public class TetrioClient : IDisposable
    {
        private const string endpoint = "http://ch.tetr.io/";
        private readonly HttpClient client;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of <see cref="TetrioClient"/>
        /// </summary>
        public TetrioClient()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(endpoint)
            };
        }

        /// <summary>
        /// Request general server statistics
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        public async Task<string> GetServerStatsAsync()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(TetrioClient));

            string requestString = "api/general/stats";
            return await client.GetStringAsync(requestString);
        }

        /// <summary>
        /// Request general server activity
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        public async Task<string> GetServerActivityAsync()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(TetrioClient));

            string requestString = "api/general/activity";
            return await client.GetStringAsync(requestString);
        }

        /// <summary>
        /// Request detailed user data
        /// </summary>
        /// <param name="user">The username or user ID to look up</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<string> GetUserInfoAsync(string user)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(TetrioClient));
            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentNullException(nameof(user));

            string requestString = $"api/users/{user.ToLower()}";

            return await client.GetStringAsync(requestString);
        }

        /// <summary>
        /// Request user's single player records
        /// </summary>
        /// <param name="user">The username or user ID to look up</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<string> GetUserRecordsAsync(string user)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(TetrioClient));
            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentNullException(nameof(user));

            string requestString = $"api/users/{user.ToLower()}/records";
            return await client.GetStringAsync(requestString);
        }

        /// <summary>
        /// Request TETRA LEAGUE leaderboard data
        /// </summary>
        /// <param name="before">The lower bound in TR. Use this to paginate upwards: take the highest seen TR and pass that back through this field to continue scrolling. If set, the search order is reversed</param>
        /// <param name="after"> The upper bound in TR. Use this to paginate downwards: take the lowest seen TR and pass that back through this field to continue scrolling</param>
        /// <param name="limit">The amount of entries to return, between 1 and 100</param>
        /// <param name="country">The ISO 3166-1 country code to filter to. Leave unset to not filter by country.</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<string> GetTLLeadearboardAsync(float before = 25000, float after = 0, int limit = 50, string country = "")
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(TetrioClient));
            if (before < 0 || before > 25000)
                throw new ArgumentOutOfRangeException(nameof(before));
            if (after < 0 || after > 25000)
                throw new ArgumentOutOfRangeException(nameof(after));
            if (limit < 1 || limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));
            if (before != 25000 && after != 0)
                throw new ArgumentException($"{nameof(before)} and {nameof(after)} parameters cannot be combined!");
            if (country is null)
                throw new ArgumentNullException(nameof(country));

            string requestString = "api/users/lists/league?";

            if (before != 25000)
                requestString += $"before={before}&";
            if (after != 0)
                requestString += $"after={after}&";
            if (limit != 50)
                requestString += $"limit={limit}&";
            if (!string.IsNullOrWhiteSpace(country))
                requestString += $"country={country}&";

            return await client.GetStringAsync(requestString[..^1]);
        }

        /// <summary>
        /// Request all users from TETRA LEAGUE leaderboard fulfilling the search criteria. 
        /// <para><b>Please do not overuse this</b></para>
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<string> GetTLLeadearboardExportAsync(string country = "")
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(TetrioClient));
            if (country is null)
                throw new ArgumentNullException(nameof(country));

            string requestString = "api/users/lists/league/all?";

            if (!string.IsNullOrWhiteSpace(country))
                requestString += $"country={country}&";

            return await client.GetStringAsync(requestString[..^1]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="before">The lower bound in XP. Use this to paginate upwards: take the highest seen XP and pass that back through this field to continue scrolling. If set, the search order is reversed</param>
        /// <param name="after">The upper bound in XP. Use this to paginate downwards: take the lowest seen XP and pass that back through this field to continue scrolling. Infinite by default.</param>
        /// <param name="limit">The amount of entries to return, between 1 and 100</param>
        /// <param name="country">The ISO 3166-1 country code to filter to. Leave unset to not filter by country.</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<string> GetXPLeadearboardExportAsync(float before = 0, float after = 0, int limit = 50, string country = "")
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(TetrioClient));
            if (before < 0)
                throw new ArgumentOutOfRangeException(nameof(before));
            if (after < 0)
                throw new ArgumentOutOfRangeException(nameof(after));
            if (limit < 1 || limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));
            if (before != 0 && after != 0)
                throw new ArgumentException($"{nameof(before)} and {nameof(after)} parameters cannot be combined!");
            if (country is null)
                throw new ArgumentNullException(nameof(country));

            string requestString = "api/users/lists/xp?";

            if (before != 0)
                requestString += $"before={before}&";
            if (after != 0)
                requestString += $"after={after}&";
            if (limit != 50)
                requestString += $"limit={limit}&";
            if (!string.IsNullOrWhiteSpace(country))
                requestString += $"country={country}&";

            return await client.GetStringAsync(requestString[..^1]);
        }

        /// <summary>
        /// Request the records in this Stream. A Stream is a list of records with a set length. Replays that are not featured in any Stream are automatically pruned
        /// </summary>
        /// <param name="stream">The stream ID to look up</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<string> GetStreamAsync(string stream)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(TetrioClient));
            if (string.IsNullOrWhiteSpace(stream))
                throw new ArgumentNullException(nameof(stream));

            string requestString = $"api/streams/{stream}";
            return await client.GetStringAsync(requestString);
        }

        /// <summary>
        /// Request latest news items in any stream
        /// </summary>
        /// <param name="limit">The amount of entries to return, between 1 and 100</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<string> GetLatestNewsAsync(int limit = 25)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(TetrioClient));
            if (limit < 1 || limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));

            string requestString = $"api/news?";

            if (limit != 25)
                requestString += $"limit={limit}&";

            return await client.GetStringAsync(requestString[..^1]);
        }

        /// <summary>
        /// Request latest news items in the stream. Use stream "global" for the global news.
        /// </summary>
        /// <param name="stream">The news stream to look up (either "global" or "user_{userID}")</param>
        /// <param name="limit">The amount of entries to return, between 1 and 100</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<string> GetLatestNewsInStreamAsync(string stream, int limit = 25)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(TetrioClient));
            if (string.IsNullOrWhiteSpace(stream))
                throw new ArgumentNullException(nameof(stream));
            if (limit < 1 || limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));

            string requestString = $"api/streams/{stream}?";

            if (limit != 25)
                requestString += $"limit={limit}&";

            return await client.GetStringAsync(requestString[..^1]);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
                client.Dispose();
                disposed = true;
            }
        }

        public void Dispose()
        {
            // Do not modify this code. Place the cleanup code in the method "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}