using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hLevelSetter : MonoBehaviour
{
    public hLevel CreateLevel(hLevel data)
    {
        hColorManager.current.SetTheme(data.difficulty, true);
        hLevel level = Instantiate(data, transform);

        level.gameBall = level.GetComponentInChildren<hGameBall>();

        return level;
    }
    #region Binary serial
    /*public hLevel CreateLevel(hLevel.SerialData serialData)
    {
        hColorManager.current.SetTheme((Difficulty)serialData.difficulty, true);

        hLevel level = serialData.DeSerialize(transform);

        level.gameObject.layer = LayerMask.NameToLayer("Outside");

        return level;
    }*/
    #endregion

    public void AdjustCamera(Camera camera, hLevel level, Transform skyBox)
    {
        //set Camera
        Collider[] colliderBounds = level.GetComponentsInChildren<Collider>();
        Bounds totalBounds = colliderBounds[1].bounds;
        for (int i = 2; i < colliderBounds.Length; ++i)
            totalBounds.Encapsulate(colliderBounds[i].bounds);

        var dir = -camera.transform.forward;
        var newPos = totalBounds.center + dir * 30f;
        camera.transform.position = newPos;

        var min = camera.WorldToScreenPoint(totalBounds.min);
        var max = camera.WorldToScreenPoint(totalBounds.max);
        min.z = 0;
        max.z = 0;
        var rightdownCorner = camera.ScreenToWorldPoint(new Vector3(max.x, min.y, 0));
        min = camera.ScreenToWorldPoint(min);
        max = camera.ScreenToWorldPoint(max);

        var width = Vector3.Distance(min, rightdownCorner);
        var height = Vector3.Distance(max, rightdownCorner);

        camera.orthographicSize = Mathf.Max(totalBounds.size.x, totalBounds.size.y) + 5f;//(width > height ? width : height);
        /*var screenRatio = Screen.height / (float)Screen.width;
        camera.orthographicSize *= screenRatio / 0.5625f;*/

        //level boundary
        var collider = level.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = level.gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
        }
        collider.size = totalBounds.size + Vector3.one * 1.5f;
        collider.center = totalBounds.center;

        if (level.wayPointList != null)
            level.wayPointList.Sort((hWayPoint a, hWayPoint b) => { if (a.id > b.id) return 1; return -1; });

        var scale = skyBox.localScale;
        scale.y = camera.orthographicSize * 2.5f;
        skyBox.localScale = scale;

        //Set gradient
        var bound = level.GetComponent<Collider>().bounds;
        var minmax = new Vector4(
            level.transform.position.y + bound.center.y - bound.extents.y,
            level.transform.position.y + bound.center.y + bound.extents.y, 0, 0);
        for (int i = 0; i < hDatabase.current.groundMats.Count; ++i)
            hDatabase.current.groundMats[i].SetVector("_MinMaxHeight", minmax);
    }
}
