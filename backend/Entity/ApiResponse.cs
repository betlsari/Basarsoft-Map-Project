<<<<<<< HEAD
namespace BasarStajApp.Entity
{
    public class ApiResponse<T> 
    {
        private bool v;
        private string pointAdded;
        private Feature addedPoint;

=======
﻿namespace BasarStajApp.Entity
{
    public class ApiResponse<T> //amacımız API'den standart bir veri yapısı döndürmek
    {
>>>>>>> dcc10a6 (son front end)
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Meta { get; set; }
        public T Data { get; set; }

        public ApiResponse(bool success, string message, T data, object meta = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Meta = meta;
        }
<<<<<<< HEAD

        public ApiResponse(bool v, string pointAdded, List<Feature> points, Feature addedPoint)
        {
            this.v = v;
            this.pointAdded = pointAdded;
            this.addedPoint = addedPoint;
        }
=======
>>>>>>> dcc10a6 (son front end)
    }
}
