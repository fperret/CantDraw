using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int m_subGoalsValidated = 0;

    static public bool m_hideSprite = false;

    public class PointGoal {

        private float m_detectionRadius;

        public Vector2 position;

        public bool    valid;

        SpriteRenderer spriteRenderer;

        public PointGoal(Vector2 sourcePos, SpriteRenderer sourceSpriteRenderer)
        {
            position = sourcePos;
            valid = false;
            spriteRenderer = sourceSpriteRenderer;            
        }

        public void updateState(bool validityState)
        {
            valid = validityState;
            if (valid)
                spriteRenderer.color = new Color(0.159f, 1, 0, 1);
            else
                spriteRenderer.color = new Color(1, 0, 0, 1);
        }

    }

    public class SubGoalData {

        public List<PointGoal> points;//

        public List<SubGoalData> subGoals;

        public bool            valid;

        public bool             mandatory;

        public float            nbOfGoalsForValid;

        // Fetched from monobehavior SubGoal.target.m_name
        public string           target;

        public SubGoalData(Transform baseTransform)
        {
            mandatory = (baseTransform.tag == "mandatoryGoal");

            points = new List<PointGoal>();
            subGoals = new List<SubGoalData>();

            bool recursiveSubGoal = false;
            foreach (Transform child in baseTransform)
            {
                if (child.TryGetComponent(out SubGoal childSubGoal))
                {
                    recursiveSubGoal = true;
                    subGoals.Add(new SubGoalData(child));
                }
                else
                {
                    if (recursiveSubGoal)
                        throw new UnityException("recursive SubGoals and PointGoals at the same level of " + child.name);

                    if (Goal.m_hideSprite)
                        child.GetComponent<SpriteRenderer>().enabled = false;

                    points.Add(new PointGoal(child.transform.position, child.GetComponent<SpriteRenderer>()));
                }
            }

            nbOfGoalsForValid = -1;
            target = "";
            if (baseTransform.TryGetComponent(out SubGoal subGoal))
            {
                if (subGoal.target != null)
                    target = subGoal.target.getName();
                if (subGoal.m_nbOfGoalsForValid >= 0)
                    nbOfGoalsForValid = subGoal.m_nbOfGoalsForValid;
            }

            if (nbOfGoalsForValid == -1)
                nbOfGoalsForValid = (float)points.Count * 0.8f;

            valid = false;
        }
    };


    [SerializeField]
    public List<SubGoalData> m_subGoals = new List<SubGoalData>();


    private int getNbOfValidPointGoals(SubGoalData subGoal, out Interactable interactableOnGoals)
    {
        interactableOnGoals = null;

        // ! This method expect only PointGoal as child so do not pass a recursive subGoal !
        int nbOfValidGoals = 0;

        foreach (PointGoal pointGoal in subGoal.points)
        {
            // Essayer OverlapCircleNonAlloc() pour optimiser les performances
            // Il faudra passer un array de collider suffisament grand pour avoir tous les resultats possibles
            //Collider2D[] overlaps = Physics2D.OverlapCircleAll(pointGoal.position, 0.2f);
            Collider2D[] overlaps = Physics2D.OverlapPointAll(pointGoal.position);
            bool check = false;
            foreach (Collider2D overlap in overlaps)
            {
                // Specific target for all the PointGoals of this SubGoal
                if (subGoal.target.Length != 0)
                {
                    //if (overlap.TryGetComponent(out Interactable interactable))
                    if (overlap.TryGetComponent(out interactableOnGoals))
                    {
                        if (interactableOnGoals.getName() == subGoal.target)
                        {
                            check = true;
                            nbOfValidGoals++;
                            break;
                        }
                    }
                }
                // Else if no target is specified we check for the FootPrints
                else if (overlap.tag == "FootPrints")
                {
                    check = true;
                    nbOfValidGoals++;
                    break ;
                }
            }
            pointGoal.updateState(check);
        }

        return nbOfValidGoals;
    }

    bool processSubGoalsWithRecursion(SubGoalData subGoalWithRecursion)
    {
        // If we want to do a real full recursive architecture we need to change something around here probably
        int nbOfValidsSubGoals = 0;
        foreach (SubGoalData recursiveSubGoal in subGoalWithRecursion.subGoals)
        {
            // Here we expect to only encounter PointGoals, else boom
            int nbOfValidGoals = getNbOfValidPointGoals(recursiveSubGoal, out Interactable interactableOnGoals);

            if (nbOfValidGoals >= recursiveSubGoal.nbOfGoalsForValid)
            {
                // If we do a true recursive thin change that
                // For now we consider that One sub-subGoal is enough to validate a subGoal
                nbOfValidsSubGoals++;
                recursiveSubGoal.valid = true;
                if (interactableOnGoals != null)
                    interactableOnGoals.validatePosition();
            }
            else
                recursiveSubGoal.valid = false;
        }
        return (nbOfValidsSubGoals >= 1);
    }

    private IEnumerator updateGoals()
    {
        while (true)
        {
            int nbOfValidSubGoals = 0;
            foreach (SubGoalData subGoal in m_subGoals)
            {
                // Recursive subgoal
                // For now we will try to not go below 1 depth of recursion
                if (subGoal.subGoals.Count != 0)
                {
                    if (processSubGoalsWithRecursion(subGoal))
                    {
                        subGoal.valid = true;
                        nbOfValidSubGoals++;
                    }
                    else
                        subGoal.valid = false;
                }
                else // "normal" subgoal
                {
                    if (getNbOfValidPointGoals(subGoal, out Interactable interactableOnGoals) > subGoal.nbOfGoalsForValid)
                    {
                        subGoal.valid = true;
                        nbOfValidSubGoals++;
                        if (interactableOnGoals != null)
                            interactableOnGoals.validatePosition();
                    }
                    else
                        subGoal.valid = false;
                }
            }
            m_subGoalsValidated = nbOfValidSubGoals;
            if (nbOfValidSubGoals == m_subGoals.Count)
            {
                GameManager.Instance.win();
                yield break;
            }
            yield return new WaitForSeconds(0.25f);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            Debug.Log(child.name);
            m_subGoals.Add(new SubGoalData(child));
        }

        StartCoroutine("updateGoals");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
