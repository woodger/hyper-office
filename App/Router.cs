namespace HyperOffice.App {
  using Nancy;

  public class Router : NancyModule {
    public Router() {
      //Controller controller;

      this.Get("/", args => "Hello World, it's Nancy on .NET Core");
    }
  }
}
