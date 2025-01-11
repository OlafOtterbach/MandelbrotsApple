import { ImageSize } from "./image-size";
import { MandelbrotSize } from "./mandelbrot-size";
import { ImageVector } from "./image-vector";

export class MandelbrotMoveParameter {
  constructor(
    public MouseVector: ImageVector,
    public ImageSize: ImageSize,
    public CurrentMandelbrotSize: MandelbrotSize,
    public MaxIterations: number) {
  }
}
