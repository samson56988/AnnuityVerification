namespace AnnuityVerification.Models
{
    public class ServiceResponse<T>
    {
        public bool Data { get; set; }
        public bool Success { get; set; } = true;
        public string? Message { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string? PhotoHash { get; set; }
        public string? Photo { get; set; }
        public T? Table { get; set; }
    }

    public class MatiAuthResponse
    {
        public string? access_token { get; set; }
        public int expiresIn { get; set; }
        public Payload? payload { get; set; }
    }

    public class Payload
    {
        public User? user { get; set; }
    }

    public class User
    {
        public string? _id { get; set; }
    }

     public class AnnuityVerificationResponse
    {
        public object[] documents { get; set; }
        public bool expired { get; set; }
        public Flow flow { get; set; }
        public Identity identity { get; set; }
        public Metadata metadata { get; set; }
        public Step[] steps { get; set; }
        public bool masJobToBePostpone { get; set; }
        public string id { get; set; }
        public Devicefingerprint deviceFingerprint { get; set; }
        public bool hasProblem { get; set; }
        public Custominputvalues customInputValues { get; set; }
    }

    public class Flow
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Identity
    {
        public string status { get; set; }
    }

    public class Metadata
    {
        public string userPhotoLink { get; set; }
        public string userPhotoLinkHash { get; set; }
    }

    public class Devicefingerprint
    {
        public string ua { get; set; }
        public Browser browser { get; set; }
        public Engine engine { get; set; }
        public Os os { get; set; }
        public Device device { get; set; }
        public App app { get; set; }
        public string ip { get; set; }
        public bool vpnDetectionEnabled { get; set; }
    }

    public class Browser
    {
        public string name { get; set; }
        public string version { get; set; }
        public string major { get; set; }
    }

    public class Engine
    {
        public string name { get; set; }
        public string version { get; set; }
    }

    public class Os
    {
        public string name { get; set; }
        public string version { get; set; }
    }

    public class Device
    {
        public string vendor { get; set; }
        public string model { get; set; }
        public string type { get; set; }
    }

    public class App
    {
        public string platform { get; set; }
        public string version { get; set; }
    }

    public class Custominputvalues
    {
        public List<Field> fields { get; set; }
        public string country { get; set; }
    }

    public class Field
    {
        public Atomicfieldparams atomicFieldParams { get; set; }
        public string _id { get; set; }
        public string name { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public bool isMandatory { get; set; }
        public object[] children { get; set; }
    }

    public class Atomicfieldparams
    {
        public string type { get; set; }
        public string regex { get; set; }
        public string placeholder { get; set; }
        public string value { get; set; }
    }

    public class Step
    {
        public int status { get; set; }
        public string id { get; set; }
        public Data data { get; set; }
        public object error { get; set; }
    }

    public class Data
    {
        public string facematchScore { get; set; }
        public Source[] sources { get; set; }
        public string videoUrl { get; set; }
        public string spriteUrl { get; set; }
        public string selfieUrl { get; set; }
    }

    public class Source
    {
        public string type { get; set; }
        public string url { get; set; }
        public bool isSpriteSet { get; set; }
    }

}
