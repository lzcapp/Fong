# Fong

A REST API service that provides access to Fing API data with SQLite caching support.

## Features

- **Fing API Integration**: Connects to Fing API to retrieve device, contact, and agent information
- **SQLite Storage**: Parsed data is automatically stored in SQLite database for improved performance
- **Flexible Data Access**: Supports both cached (database) and live (API) data retrieval
- **RESTful API**: Clean REST endpoints for all data types
- **Cache Management**: Built-in cache refresh capabilities

## API Endpoints

### Devices
- `GET /api/devices` - Get all devices
- `GET /api/devices/online` - Get only active/online devices

### Contacts
- `GET /api/contacts` - Get all contacts

### Agent Info
- `GET /api/agentinfo` - Get agent information

### Cache Management
- `POST /api/cache/refresh` - Refresh all cached data from API

### Query Parameters
All data endpoints support the following query parameters:
- `useCache=true/false` - Whether to use cached data (default: true)
- `refresh=true/false` - Whether to refresh cache from API (default: false)

## Configuration

### Database
The application uses SQLite for data storage. Connection strings are configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=fong.db"
  }
}
```

### Fing API Settings
Configure Fing API access in `appsettings.json`:

```json
{
  "FingApiSettings": {
    "ApiHost": "your-fing-host",
    "ApiPort": "your-fing-port", 
    "ApiKey": "your-api-key"
  }
}
```

Or use environment variables:
- `FING_API_HOST`
- `FING_API_PORT`
- `FING_API_KEY`

## Data Storage

The application automatically:
1. Attempts to serve data from SQLite cache when available
2. Falls back to Fing API if cache is empty or refresh is requested  
3. Stores all API responses in SQLite for future use
4. Updates existing records and adds new ones during bulk operations

### Database Schema

Three main tables store the parsed data:
- **Devices**: Network device information with unique MAC address constraint
- **Contacts**: Contact information with unique ContactId constraint
- **AgentInfo**: Agent status information (only latest record kept)

## Development

### Prerequisites
- .NET 8.0 SDK
- SQLite

### Running the Application
```bash
dotnet run
```

The database will be automatically created on first run.

### Building
```bash
dotnet build
```

## Docker Support

The application includes Dockerfile for containerized deployment.
