using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JorgeJGnz
{
    [System.Serializable]
    public class Drawing
    {
        public static float defaultBoundsTheresold = 1f;

        public string drawingName;
        public Vector3 relativeFrom;
        public List<Vector3> relativePositions;
        public Bounds bounds;
        public float boundsTheresold;
        public UnityEvent onRecognized;

        public Drawing(string name, Vector3 relativeFrom, List<Vector3> positions, UnityEvent onRecognized)
        {
            this.drawingName = name;
            this.relativeFrom = relativeFrom;
            this.relativePositions = positions;
            this.bounds = new Bounds();
            this.boundsTheresold = defaultBoundsTheresold;
            this.onRecognized = onRecognized;
        }

        public Drawing(string name, Vector3 relativeFrom, List<Vector3> positions)
        {
            this.drawingName = name;
            this.relativeFrom = relativeFrom;
            this.relativePositions = positions;
            this.bounds = new Bounds();
            this.boundsTheresold = defaultBoundsTheresold;
            this.onRecognized = new UnityEvent();
        }

        public Drawing(Vector3 relativeFrom, List<Vector3> positions)
        {
            this.drawingName = "Drawn";
            this.relativeFrom = relativeFrom;
            this.relativePositions = positions;
            this.bounds = new Bounds();
            this.boundsTheresold = defaultBoundsTheresold;
            this.onRecognized = new UnityEvent();
        }

        public Drawing()
        {

        }

        public bool Equals(Drawing d)
        {
            // If it looks like a duck, swims like a duck, and quacks like a duck, then it probably is a duck
            if (this.drawingName == d.drawingName
                && this.relativeFrom == d.relativeFrom
                && this.relativePositions == d.relativePositions
                && this.bounds == d.bounds
                && this.boundsTheresold == d.boundsTheresold
                && this.onRecognized == d.onRecognized) return true;
            else return false;
        }

        public void RecalculateBounds()
        {
            this.bounds = CalculateBounds(this.relativePositions);
            Debug.Log("(RecalculateBounds) Drawing " + this.drawingName + " | Centroid: " + this.bounds.center + " | Size: " + this.bounds.size);
        }

        Bounds CalculateBounds(List<Vector3> points)
        {
            List<float> allXpos = new List<float>();
            List<float> allYpos = new List<float>();
            List<float> allZpos = new List<float>();

            foreach (Vector3 v in points)
            {
                allXpos.Add(v.x);
                allYpos.Add(v.y);
                allZpos.Add(v.z);
            }

            // We discard Y axis as they are 2D drawings. (maxY = 1) - (minY = 0) = 0 -> CalculateVector3Volume X * 1f * Z != 0
            Vector3 max = new Vector3(Mathf.Max(allXpos.ToArray()), 1f, Mathf.Max(allZpos.ToArray()));
            Vector3 min = new Vector3(Mathf.Min(allXpos.ToArray()), 0f, Mathf.Min(allZpos.ToArray()));

            return new Bounds(findCentroid(points), max - min);
        }

        Vector3 findCentroid(List<Vector3> points)
        {
            Vector3 centroid = new Vector3(0, 0, 0);
            int numPoints = points.Count;
            foreach (Vector3 point in points)
            {
                centroid += point;
            }

            centroid /= numPoints;

            return centroid;
        }

    }

    [System.Serializable]
    public class FingerDrawer
    {
        public GameObject gameObject;
        public bool canDraw;
        public bool force;
    }

    // [ExecuteInEditMode] (for debugging and saving new letters)
    public class DrawingDetector : MonoBehaviour
    {
        [Header("Drawings")]
        [SerializeField]
        public List<Drawing> savedDrawings = new List<Drawing>();

        [Header("Detection")]
        public float theresold = 1.0f;

        public UnityEvent onNothindDetected;

        [HideInInspector]
        public Drawing drawingDetected;
        bool sthWasDetected = true;
        bool loading = false;

        [Header("Objects")]
        public List<FingerDrawer> fingerDrawers;
        public BoxCollider drawingTrigger;
        public GameObject relativeFrom;
        public GameObject drawingsContainer;
        GameObject temporalPointsContainer;
        List<GameObject> paintedDrawings;

        // drawingsContainer
        // |-temporalPointsContainer
        // | |-paintedPoint
        // | |-paintedPoint
        // | |-...
        // |-savedA
        // | |-paintedPoint
        // | |-paintedPoint
        // | |-...
        // |-savedB
        // | |-paintedPoint
        // | |-paintedPoint
        // | |-...

        // paintedDrawings = [ savedA, savedB ]

        List<Vector3> drawnPositions = new List<Vector3>();

        [Header("Debugging")]
        public string drawingNameDetected = "";
        public bool showDrawing = true;
        public GameObject prefabPoint;
        public TextMeshPro debugPanel;
        string extra = "";
        [Tooltip("To ensure acceptable performance")]
        public int maxDrawnPoints = 500;

        [Header("Saving")]
        public string drawingName = "";
        public int savePositionEveryFrames = 2;
        int framesUntilNextSaving;

        bool drawing = false;
        bool alreadyStarted = false;

        // Start is called before the first frame update
        void Start()
        {
            foreach (Drawing d in savedDrawings)
            {
                d.RecalculateBounds();
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Performance limitation
            if (drawnPositions.Count > maxDrawnPoints) StopDrawing();

            if (drawing)
            {
                if (framesUntilNextSaving == 0)
                {
                    RefreshFingerDrawersPermissions();

                    // Multiple drawers
                    foreach (FingerDrawer fd in fingerDrawers)
                    {
                        if (fd.canDraw)
                        {
                            // Add position
                            drawnPositions.Add(relativeFrom.transform.InverseTransformPoint(fd.gameObject.transform.position));

                            framesUntilNextSaving = savePositionEveryFrames;

                            // Paint poistion
                            GameObject p = Instantiate(prefabPoint, fd.gameObject.transform.position, Quaternion.identity);
                            p.transform.parent = temporalPointsContainer.transform;

                            if (!showDrawing) p.SetActive(false);
                        }
                    }

                }
                else framesUntilNextSaving--;
            }

            // Debugging
            debugPanel.text = "Drawing? " + drawing + " | AlreadyStarted? " + alreadyStarted + "\n";
            foreach (FingerDrawer fd in fingerDrawers)
            {
                debugPanel.text += "[FingerDrawer " + fd.gameObject.name + "] can Draw? " + fd.canDraw + "\n";
            }
            debugPanel.text += extra;

        }

        public void StartDrawing()
        {
            // Erase previous painted points
            if (temporalPointsContainer != null) Destroy(temporalPointsContainer);

            // Recreate the temporal point container
            temporalPointsContainer = new GameObject();
            temporalPointsContainer.transform.parent = drawingsContainer.transform;
            temporalPointsContainer.name = "TemporalPointsContainer";

            // Refresh
            drawnPositions = new List<Vector3>();
            framesUntilNextSaving = savePositionEveryFrames;

            // We can stop/resume
            alreadyStarted = true;

            // Enable drawing
            drawing = true;
        }

        public void StopDrawing()
        {
            // Disable drawing
            drawing = false;
        }

        public void ResumeDrawing()
        {
            // Re-enable drawing
            if (alreadyStarted) drawing = true;
        }

        public void SaveDrawing()
        {
            // Build the drawing
            Drawing d = new Drawing(drawingName, relativeFrom.transform.position, drawnPositions);

            // Save it
            savedDrawings.Add(d);

            // Rename group of painted points
            GameObject paintedDrawing = Instantiate(temporalPointsContainer, temporalPointsContainer.transform.position, Quaternion.identity);
            paintedDrawing.name = drawingName;
            paintedDrawing.transform.parent = drawingsContainer.transform;
            paintedDrawing.SetActive(false);

            // We cannot resume
            alreadyStarted = false;
        }

        public void RestartDrawing()
        {
            StopDrawing();
            StartDrawing();
        }

        public void CheckDrawingAndRestart()
        {
            // This will change if we make drawingChecker asynchronous
            drawingChecker();
            RestartDrawing();
        }

        public void CheckDrawing()
        {
            loading = true;
            //StartCoroutine(drawingChecker());
            drawingChecker();
        }

        //IEnumerator drawingChecker()
        void drawingChecker()
        {
            //yield return drawingDetected = Recognize(drawnPositions);
            drawingDetected = Recognize(drawnPositions);

            if (drawingDetected.Equals(new Drawing()) && sthWasDetected)
            {
                sthWasDetected = false;
                drawingNameDetected = "NaN";
                onNothindDetected.Invoke();
            }
            else if (!drawingDetected.Equals(new Drawing()))
            {
                sthWasDetected = true;
                drawingNameDetected = drawingDetected.drawingName;
                drawingDetected.onRecognized.Invoke();
            }

            loading = false;
        }

        Drawing Recognize(List<Vector3> drawnPositions)
        {
            bool discardDrawing = false;
            List<Vector3> orderedPositions;
            float sumDistances;
            float minSumDistances = Mathf.Infinity;
            Drawing bestCandidate = new Drawing();

            float volumeDesired = 0f, volumeDrawn = 0f, volumeDifference = 0f;

            // Build the drawing
            Drawing d = new Drawing(relativeFrom.transform.position, drawnPositions);
            d.RecalculateBounds();

            // For each saved-drawing
            for (int i = 0; i < savedDrawings.Count; i++)
            {
                // Check if drawing bounds is close to the desired
                volumeDesired = GetVector3Volume(savedDrawings[i].bounds.size) * 1000f;
                volumeDrawn = GetVector3Volume(d.bounds.size) * 1000f;
                volumeDifference = Mathf.Abs(volumeDesired - volumeDrawn);

                //Debugging
                extra = "Delta volume (" + savedDrawings[i].drawingName + "): " + volumeDifference + "\n";
                extra += "Theresold: " + savedDrawings[i].boundsTheresold + " | Delta > Theresold? " + (volumeDifference > savedDrawings[i].boundsTheresold);

                if (volumeDifference > savedDrawings[i].boundsTheresold)
                {
                    // If not, we skip the drawing
                    continue;
                }

                sumDistances = 0f;

                // For each drawn position
                for (int j = 0; j < drawnPositions.Count; j++)
                {
                    // Get the list of the saved-drawing positions ordered by their distance to the drawn positions we want to compare
                    orderedPositions = savedDrawings[i].relativePositions.OrderBy(
                        x => Vector3.Distance(drawnPositions[j], x)
                    ).ToList();

                    // If the drawn position does not enter the theresold of the closest saved-drawing position then we discard the saved-drawing
                    if (Vector3.Distance(drawnPositions[j], orderedPositions[0]) > theresold)
                    {
                        discardDrawing = true;
                        break;
                    }

                    // If all the drawn positions entered, then we calculate the total of their distances
                    sumDistances += Vector3.Distance(drawnPositions[j], orderedPositions[0]);
                }

                // If we have to discard the saved-drawing, we skip it
                if (discardDrawing)
                {
                    discardDrawing = false;
                    continue;
                }

                // If it is valid and the sum of its distances is less than the existing record, it is replaced because it is a better candidate 
                if (sumDistances < minSumDistances)
                {
                    minSumDistances = sumDistances;
                    bestCandidate = savedDrawings[i];
                }
            }

            // If we've found something, we'll return it
            // If we haven't found anything, we return it anyway (newly created object)
            return bestCandidate;
        }

        public void RefreshFingerDrawersPermissions()
        {
            foreach (FingerDrawer fd in fingerDrawers)
            {
                if (!fd.force)
                {
                    if (drawingTrigger.bounds.Contains(fd.gameObject.transform.position)) fd.canDraw = true;
                    else fd.canDraw = false;
                }
            }
        }

        float GetVector3Volume(Vector3 vector)
        {
            return vector.x * vector.y * vector.z;
        }

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DrawingDetector))]
    public class CustomInspector_DrawingDetector : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DrawingDetector myScript = (DrawingDetector)target;

            if (GUILayout.Button("Start drawing"))
            {
                myScript.StartDrawing();
                OnInspectorGUI();
            }

            if (GUILayout.Button("Stop drawing"))
            {
                myScript.StopDrawing();
                OnInspectorGUI();
            }

            if (GUILayout.Button("Resume drawing"))
            {
                myScript.ResumeDrawing();
                OnInspectorGUI();
            }

            if (GUILayout.Button("Restart drawing"))
            {
                myScript.RestartDrawing();
                OnInspectorGUI();
            }

            if (GUILayout.Button("Save drawing"))
            {
                myScript.SaveDrawing();
                OnInspectorGUI();
            }

            if (GUILayout.Button("Check drawing"))
            {
                myScript.CheckDrawing();
                OnInspectorGUI();
            }

        }
    }
#endif
}
