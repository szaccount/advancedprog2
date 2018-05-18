using System;
using System.Collections.Generic;
using System.Text;

namespace ImageService.Communication
{
    public interface IServerChannel
    {
        void Start();

        void Stop();
    }
}
