🚀 Blocked Countries API (.NET 8)

A lightweight .NET 8 Web API that manages blocked countries and validates IP addresses using third-party geolocation services ( IPGeolocation.io ).
This project is designed for learning and demonstration purposes — showing how to build a scalable, thread-safe API using in-memory data storage instead of a database.

🧩 Key Features
🗺️ Country Blocking

Add / Remove Blocked Countries (permanent or temporary)

In-Memory Storage using ConcurrentDictionary

Pagination and Filtering for GET /api/countries/blocked

⏱️ Temporary Blocking

Block any country for a specific duration (1–1440 minutes).

Automatic cleanup every 5 minutes using a Background Service.

🌐 IP Validation & Lookup

Validate IP format before lookup (IPAddress.TryParse)

Fetch country details using a third-party API.

Auto-detect caller IP via HttpContext (with fallback for local testing).

🧠 Logging & Monitoring

Logs each (blocked IP ) request that checks if an IP is blocked.

Stores IP address, timestamp, country code, user agent, and blocked status in memory.



🧩 Future Enhancements

Unify permanent and temporary blocked countries into a single entity.

Add caching layer for IP lookups.

Integrate real logging (e.g., Serilog + file sink).

Add unit tests for store & validation logic.
More integration for Phone number information ... .

Optional database version (EF Core / Redis).
