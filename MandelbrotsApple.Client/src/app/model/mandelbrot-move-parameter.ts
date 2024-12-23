import { CanvasSize } from "./canvas-size";
import { MandelbrotSize } from "./mandelbrot-size";
import { CanvasVector } from "./canvas-vector";

export class MandelbrotMoveParameter {
  constructor(
    public MouseVector: CanvasVector,
    public CanvasSize: CanvasSize,
    public CurrentMandelbrotSize: MandelbrotSize,
    public MaxIterations: number) {
  }
}
