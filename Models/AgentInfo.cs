using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Fong.Models {
    public class AgentInfo {
        public string Ip { get; private set; }
        public string ModelName { get; private set; }
        public string State { get; private set; }
        public string AgentId { get; private set; }
        public string FriendlyName { get; private set; }
        public string DeviceType { get; private set; }
        public string Manufacturer { get; private set; }

        public AgentInfo(string response) {
            var ns = "urn:schemas-upnp-org:device-1-0";
            var doc = XDocument.Parse(response);
            var root = doc.Root;
            if (root == null) return;

            var urlBase = root.Element(XName.Get("URLBase", ns));
            if (urlBase != null)
                Ip = urlBase.Value.Split(':')[0];

            var device = root.Element(XName.Get("device", ns));
            if (device != null) {
                FriendlyName = device.Element(XName.Get("friendlyName", ns))?.Value;
                ModelName = device.Element(XName.Get("modelName", ns))?.Value;
                DeviceType = device.Element(XName.Get("deviceType", ns))?.Value?.Replace("urn:fing:", "")
                    .Replace("urn:domotz:", "");
                Manufacturer = device.Element(XName.Get("manufacturer", ns))?.Value;

                var serviceList = device.Element(XName.Get("serviceList", ns));
                if (serviceList != null) {
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
    }
}