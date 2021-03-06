import {Format} from "../format";
import {Category} from "../category.model";
import {Limit} from "../limit.model";
import {SubCategory} from "../subcategory.model";
import {Attribute} from "../attribute.model";
import {Type} from "../type.model";

export class DeckCardSearchModel {
  banlist: Format;
  category: Category;
  subCategory: SubCategory;
  attribute: Attribute;
  type: Type;
  lvlrank: number;
  limit: Limit;
  atk: number = 0;
  def: number = 0;
  searchText: string;
  pageSize: number = 10;
  pageIndex: number = 1;

  constructor(values: Object = {}) {
    Object.assign(this, values);
  }
}
