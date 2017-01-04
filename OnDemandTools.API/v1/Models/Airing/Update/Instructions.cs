namespace OnDemandTools.API.v1.Models.Airing.Update
{
    public class Instructions
    {
        public Instructions()
        {
            PerformDeliveryValidation = true;
            DeliverImmediately = false;
            DisableTracking = false;
        }

        public bool DeliverImmediately { get; set; }

        public bool PerformDeliveryValidation { get; set; }

        public bool DisableTracking { get; set; }
    }
}