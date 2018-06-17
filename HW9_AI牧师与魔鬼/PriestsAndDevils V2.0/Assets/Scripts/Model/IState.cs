using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

namespace MyNamespace {

  public class IState {
    public int leftPriests;
    public int leftDevils;
    public int rightPriests;
    public int rightDevils;
    public bool boat;
    public IState parent;

    public IState() {
      this.leftDevils = 0;
      this.leftPriests = 0;
      this.rightDevils = 0;
      this.rightPriests = 0;
      this.parent = null;
    }

    public IState(int leftPriests, int leftDevils, int rightPriests, int rightDevils, bool boat, IState parent) {
      this.leftPriests = leftPriests;
      this.leftDevils = leftDevils;
      this.rightPriests = rightPriests;
      this.rightDevils = rightDevils;
      this.boat = boat;
      this.parent = parent;
    }

    public IState(IState another) {
      this.leftPriests = another.leftPriests;
      this.leftDevils = another.leftDevils;
      this.rightPriests = another.rightPriests;
      this.rightDevils = another.rightDevils;
      this.boat = another.boat;
      this.parent = another.parent;
    }


    public static bool operator ==(IState lhs, IState rhs) {
      return (lhs.leftPriests == rhs.leftPriests && lhs.leftDevils == rhs.leftDevils &&
        lhs.rightPriests == rhs.rightPriests && lhs.rightDevils == rhs.rightDevils &&
        lhs.boat == rhs.boat);
    }

    public static bool operator !=(IState lhs, IState rhs) {
      return !(lhs == rhs);
    }

    public override bool Equals(object obj) {
      if (obj == null) {
        return false;
      }
      if (obj.GetType().Equals(this.GetType()) == false) {
        return false;
      }
      IState temp = null;
      temp = (IState)obj;
      return this.leftPriests.Equals(temp.leftPriests) &&
        this.leftDevils.Equals(temp.leftDevils) &&
        this.rightDevils.Equals(temp.rightDevils) &&
        this.rightPriests.Equals(temp.rightPriests) &&
        this.boat.Equals(temp.boat);
    }

    public override int GetHashCode() {
      return this.leftDevils.GetHashCode() + this.leftPriests.GetHashCode() +
        this.rightDevils.GetHashCode() + this.rightPriests.GetHashCode() +
        this.boat.GetHashCode();
    }

    public bool valid() {
      return ((this.leftPriests == 0 || this.leftPriests >= this.leftDevils) &&
        (this.rightPriests == 0 || this.rightPriests >= this.rightDevils));
    }

    public static IState bfs(IState start, IState end) {
      Queue<IState> found = new Queue<IState>();
      Queue<IState> visited = new Queue<IState>();
      IState temp = new IState(start.leftPriests, start.leftDevils, start.rightPriests, start.rightDevils, start.boat, null);
      found.Enqueue(temp);

      while (found.Count > 0) {
        temp = found.Peek();

        if (temp == end) {
          //Debug.Log("solution path:\n");
          while (temp.parent != start) {
            temp = temp.parent;
          }
          return temp;
        }

        found.Dequeue();
        visited.Enqueue(temp);


        // next node
        if (temp.boat) {
          // one move to right
          if (temp.leftPriests > 0) {
            IState next = new IState(temp);
            next.parent = new IState(temp);
            next.boat = false;
            next.leftPriests--;
            next.rightPriests++;
            if (next.valid() && !visited.Contains(next) && !found.Contains(next)) {
              found.Enqueue(next);
            }
          }
          if (temp.leftDevils > 0) {
            IState next = new IState(temp);
            next.parent = new IState(temp);
            next.boat = false;
            next.leftDevils--;
            next.rightDevils++;
            if (next.valid() && !visited.Contains(next) && !found.Contains(next)) {
              found.Enqueue(next);
            }
          }
          // two moves to right
          if (temp.leftDevils > 0 && temp.leftDevils > 0) {
            IState next = new IState(temp);
            next.parent = new IState(temp);
            next.boat = false;
            next.leftPriests--;
            next.leftDevils--;
            next.rightPriests++;
            next.rightDevils++;
            if (next.valid() && !visited.Contains(next) && !found.Contains(next)) {
              found.Enqueue(next);
            }
          }
          if (temp.leftDevils > 1) {
            IState next = new IState(temp);
            next.parent = new IState(temp);
            next.boat = false;
            next.leftDevils -= 2;
            next.rightDevils += 2;
            if (next.valid() && !visited.Contains(next) && !found.Contains(next)) {
              found.Enqueue(next);
            }
          }
          if (temp.leftPriests > 1) {
            IState next = new IState(temp);
            next.parent = new IState(temp);
            next.boat = false;
            next.leftPriests -= 2;
            next.rightPriests += 2;
            next.parent = new IState(temp);
            if (next.valid() && !visited.Contains(next) && !found.Contains(next)) {
              found.Enqueue(next);
            }
          }
        } else {
          //one move to left
          if (temp.rightPriests > 0) {
            IState next = new IState(temp);
            next.parent = new IState(temp);
            next.boat = true;
            next.rightPriests--;
            next.leftPriests++;
            if (next.valid() && !visited.Contains(next) && !found.Contains(next)) {
              found.Enqueue(next);
            }
          }
          if (temp.rightDevils > 0) {
            IState next = new IState(temp);
            next.parent = new IState(temp);
            next.boat = true;
            next.rightDevils--;
            next.leftDevils++;
            if (next.valid() && !visited.Contains(next) && !found.Contains(next)) {
              found.Enqueue(next);
            }
          }
          //two moves to left
          if (temp.rightDevils > 0 && temp.rightDevils > 0) {
            IState next = new IState(temp);
            next.parent = new IState(temp);
            next.boat = true;
            next.rightPriests--;
            next.rightDevils--;
            next.leftPriests++;
            next.leftDevils++;
            if (next.valid() && !visited.Contains(next) && !found.Contains(next)) {
              found.Enqueue(next);
            }
          }
          if (temp.rightDevils > 1) {
            IState next = new IState(temp);
            next.parent = new IState(temp);
            next.boat = true;
            next.rightDevils -= 2;
            next.leftDevils += 2;
            if (next.valid() && !visited.Contains(next) && !found.Contains(next)) {
              found.Enqueue(next);
            }
          }
          if (temp.rightPriests > 1) {
            IState next = new IState(temp);
            next.parent = new IState(temp);
            next.boat = true;
            next.rightPriests -= 2;
            next.leftPriests += 2;
            if (next.valid() && !visited.Contains(next) && !found.Contains(next)) {
              found.Enqueue(next);
            }
          }
        }
      }
      return null;
    }
  }
}

