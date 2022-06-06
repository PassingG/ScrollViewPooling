using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wise.ScrollViewPooling
{
    public interface IScrollViewInit
    {
        void InitScrollviewPooling();
        void SetViewScrollView(int dataIndex, int objectIndex);
    }
}