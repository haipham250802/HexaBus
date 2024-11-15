using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public static class GameHelper
{
    private static System.Random random = new System.Random();
    public static Vector3 GetPointDistanceFromObject(float distance, Vector3 direction, Vector3 fromPoint)
    {
        distance -= 1;
        //if (distance < 0)
        //    distance = 0;

        Vector3 finalDirection = direction + direction.normalized * distance;
        Vector3 targetPosition = fromPoint + finalDirection;

        return targetPosition;
    }
    public static List<T> ShuffleBySegments<T>(List<T> list, List<int> segments)
    {
        List<T> shuffledList = new List<T>(list); // Tạo bản sao của danh sách gốc
        int startIndex = 0;

        foreach (int segmentSize in segments)
        {
            if (startIndex + segmentSize > shuffledList.Count)
                break;

            // Tạo một đoạn con của danh sách và xáo trộn nó
            List<T> segment = shuffledList.Skip(startIndex).Take(segmentSize).ToList();
            segment = segment.OrderBy(_ => random.Next()).ToList();

            // Thay thế các phần tử trong danh sách đã sao chép bằng đoạn đã xáo trộn
            for (int i = 0; i < segmentSize; i++)
            {
                shuffledList[startIndex + i] = segment[i];
            }

            startIndex += segmentSize;
        }

        return shuffledList; // Trả về danh sách đã xáo trộn
    }
    public static void ShuffleBySegments<T>( ref List<T> list, List<int> segments)
    {
        int startIndex = 0;

        foreach (int segmentSize in segments)
        {
            if (startIndex + segmentSize > list.Count)
                break;

            // Shuffle đoạn từ startIndex đến (startIndex + segmentSize - 1)
            ShuffleSegment(ref list, startIndex, segmentSize);
            startIndex += segmentSize;
        }
    }

    public static void ShuffleSegment<T>(ref List<T> list, int startIndex, int segmentSize)
    {
        for (int i = startIndex; i < startIndex + segmentSize - 1; i++)
        {
            int j = random.Next(i, startIndex + segmentSize);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    public static List<T> MoveElementToRandomPosition<T>(List<T> list, int position)
    {
        // Kiểm tra nếu vị trí hợp lệ
        if (position < 0 || position >= list.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Vị trí không hợp lệ.");
        }

        // Tạo một bản sao của danh sách để tránh thay đổi danh sách gốc
        List<T> result = new List<T>(list);
        System.Random random = new System.Random();

        // Lấy phần tử tại vị trí chỉ định
        T element = result[position];

        // Xóa phần tử khỏi vị trí hiện tại
        result.RemoveAt(position);

        // Chọn vị trí ngẫu nhiên mới
        int newIndex = random.Next(result.Count + 1);

        // Chèn phần tử vào vị trí mới
        result.Insert(newIndex, element);

        return result;
    }
    public static List<T> ShuffleList<T>(List<T> list)
    {
        return list.OrderBy(x => Guid.NewGuid()).ToList();
    }
    public static Vector3 GetDirectionFromAngle(Vector3 vectorP, float angle, Vector3 positionStart)
    {
        Vector3 H = GetPointDistanceFromObject(1, vectorP, positionStart);
        float y = Vector2.Distance(H, positionStart) * Mathf.Tan(angle * Mathf.PI / 180f);

        Vector3 u = H - positionStart;
        Vector3 n = new Vector3(u.y, -u.x);

        Vector3 C = GetPointDistanceFromObject(y, -n, H);

        return Vector3.Normalize(C - positionStart);
    }
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
