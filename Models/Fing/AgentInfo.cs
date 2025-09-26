using System.Xml.Linq;

namespace Fong.Models.Fing {
    public class AgentInfo {
        public string Ip { get; private set; } = string.Empty;
        public string ModelName { get; private set; } = string.Empty;
        public string State { get; private set; } = string.Empty;
        public string AgentId { get; private set; } = string.Empty;
        public string FriendlyName { get; private set; } = string.Empty;
        public string DeviceType { get; private set; } = string.Empty;
        public string Manufacturer { get; private set; } = string.Empty;

        public AgentInfo(string response) {
            const string ns = "urn:schemas-upnp-org:device-1-0";
            var doc = XDocument.Parse(response);
            var root = doc.Root;
            if (root == null) return;

            var urlBase = root.Element(XName.Get("URLBase", ns));
            if (urlBase != null)
                Ip = urlBase.Value.Split(':')[0];

            var device = root.Element(XName.Get("device", ns));
            if (device == null) {
                return;
            }

            FriendlyName = device.Element(XName.Get("friendlyName", ns))?.Value ?? string.Empty;
            ModelName = device.Element(XName.Get("modelName", ns))?.Value ?? string.Empty;
            DeviceType = device.Element(XName.Get("deviceType", ns))?.Value.Replace("urn:fing:", "").Replace("urn:domotz:", "") ?? string.Empty;
            Manufacturer = device.Element(XName.Get("manufacturer", ns))?.Value ?? string.Empty;

            var serviceList = device.Element(XName.Get("serviceList", ns));
            if (serviceList == null) {
                return;
            }

            foreach (var service in serviceList.Elements(XName.Get("service", ns))) {
                var serviceType = service.Element(XName.Get("serviceType", ns))?.Value;
                if (serviceType == null) continue;

                if (serviceType.StartsWith("urn:fing:device:fingagent:mac:"))
                    AgentId = serviceType.Replace("urn:fing:device:fingagent:mac:", "");
                else if (serviceType.StartsWith("urn:domotz:device:fingbox:mac:"))
                    AgentId = serviceType.Replace("urn:domotz:device:fingbox:mac:", "");

                if (serviceType.Contains(":active:1"))
                    State = "active";
                else if (serviceType.Contains(":inactive:"))
                    State = "inactive";
                else if (serviceType.Contains(":unknown:1"))
                    State = "unknown";
            }
        }
    }
}