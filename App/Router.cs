using Nancy;

namespace HyperOffice.App {
  public class Router : NancyModule {
    public Router() {
      Get("/", args => "Hello World, it's Nancy on .NET Core");
    }
  }
}
