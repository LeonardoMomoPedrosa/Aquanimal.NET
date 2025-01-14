using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StackExchange.Redis;

public partial class Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("aquaelasticcache-vdrfff.serverless.use1.cache.amazonaws.com:6379,abortConnect=false,syncTimeout=10000,connectTimeout=10000");
        // Get a reference to the Redis database
        IDatabase db = redis.GetDatabase();
        // ... Your Redis operations go here ...
        // Disconnect from Redis
        db.StringSet("hello", "worlddd");

        Label_result.InnerText = db.StringGet("hello");
        redis.Close();
    }
}
