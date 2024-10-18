import { ErrorType } from "./error-type";

export interface MandelbrotResult {
  ImageData: string,
  Errors: ErrorType[],
  HasErrors: boolean
}
