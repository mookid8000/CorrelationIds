# Correlation IDs

Demo that shows how two Web APIs and two Rebus endpoints can communicate, totally oblivious of the
fact that what they do (to eachother) will be tracked by correlation IDs!

Run the demo by

* make sure `Config.SqlServer` points to a SQL Server database that Rebus can use (defaults to local SQLEXPRESS with a `corr_ids` database)
* configure everything to be started when you press F5

and then you fire up [Postman] or figure out some other way of issuing a POST to http://localhost:64077/api/dostuff/justdoit 
to trigger some action.


[Postman]: https://chrome.google.com/webstore/detail/postman/fhbjgbiflinjbdggehcddcbncdddomop