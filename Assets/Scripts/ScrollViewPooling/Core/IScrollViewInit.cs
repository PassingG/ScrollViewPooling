using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScrollViewInit
{
    void InitScrollviewPooling();
    void SetViewRank(int dataIndex, int objectIndex);
}
